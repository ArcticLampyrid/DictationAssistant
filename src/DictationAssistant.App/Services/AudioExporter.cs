using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;
using System.Diagnostics;

namespace DictationAssistant.App.Services;

public sealed class AudioExporter
{
    private readonly IVoice _voice;
    private readonly IWordListSource _wordListSource;

    public AudioExporter(IVoice voice, IWordListSource wordListSource, IWaitingTimeCalculator? waitingTimeCalculator = null)
    {
        _voice = voice;
        _wordListSource = wordListSource;
    }

    public async Task<SaveAudioResult> ExportAudioAsync(
        SaveAudioRequest request,
        int timesPerWord,
        int volume,
        int rate,
        IWaitingTimeCalculator? waitingTimeCalculator = null,
        CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            return SaveAudioResult.Success("没有词语可导出");
        }

        if (string.IsNullOrWhiteSpace(request.OutputPath))
        {
            return SaveAudioResult.NotSupported("请指定输出路径");
        }

        var format = request.OutputFormat?.ToLowerInvariant() ?? "wav";
        if (format != "wav" && format != "mp3" && format != "opus")
        {
            return SaveAudioResult.NotSupported($"不支持的格式: {format}");
        }

        var generateLrc = request.LyricMode == "Lrc File" && !string.IsNullOrWhiteSpace(request.LyricsOutputPath);
        var progress = request.Progress;
        ILyricWriter? lyricWriter = null;

        try
        {
            await using var pcmStream = new MemoryStream();

            var targetFormat = new PcmFormatInfo(request.SampleRate, request.Channels, PcmSampleFormat.S16LE);
            using var pcmWriter = new PcmWriter(targetFormat, pcmStream);

            var totalWords = _wordListSource.Count;
            var totalSegments = totalWords * timesPerWord;
            var currentSegment = 0;
            long byteOffset = 0;

            if (generateLrc)
            {
                lyricWriter = new LyricWriter(request.LyricsOutputPath!);
            }

            for (var index = 0; index < totalWords; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var word = _wordListSource.GetWordAt(index);

                for (var repeat = 1; repeat <= timesPerWord; repeat++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var options = new VoiceSynthesisOptions
                    {
                        Rate = rate
                    };

                    var pcmAudio = await _voice.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);

                    if (pcmAudio is not null)
                    {
                        if (generateLrc && repeat == 1)
                        {
                            lyricWriter?.WriteTimestamp(pcmWriter.BytesToMilliseconds(byteOffset), word);
                        }

                        byteOffset += pcmWriter.Write(pcmAudio);
                    }

                    if (repeat < timesPerWord || index < totalWords - 1)
                    {
                        var silenceMs = GetWaitingSeconds(word, waitingTimeCalculator) * 1000;
                        if (silenceMs > 0 && pcmAudio is not null)
                        {
                            byteOffset += pcmWriter.WriteDelay(silenceMs);
                        }
                    }

                    currentSegment++;
                    progress?.Report((double)currentSegment / totalSegments);
                }
            }

            if (generateLrc && lyricWriter is not null)
            {
                lyricWriter.Flush();
            }

            var pcmData = pcmStream.ToArray();

            if (format == "wav")
            {
                await using var outputStream = new FileStream(request.OutputPath, FileMode.Create, FileAccess.Write);
                WavWriter.WriteHeader(outputStream, request.SampleRate, request.Channels, pcmData.Length);
                await outputStream.WriteAsync(pcmData, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await using var encoder = new FFmpegAudioEncoder(request.OutputPath, request.SampleRate, request.Channels, format);
                await encoder.InputStream.WriteAsync(pcmData, cancellationToken).ConfigureAwait(false);
                await encoder.FinishAsync(cancellationToken).ConfigureAwait(false);
            }

            progress?.Report(1.0);
            return SaveAudioResult.Success($"已导出到: {request.OutputPath}");
        }
        catch (OperationCanceledException)
        {
            if (File.Exists(request.OutputPath))
            {
                try
                {
                    File.Delete(request.OutputPath);
                }
                catch
                {
                }
            }

            if (generateLrc && File.Exists(request.LyricsOutputPath))
            {
                try
                {
                    File.Delete(request.LyricsOutputPath);
                }
                catch
                {
                }
            }

            return SaveAudioResult.NotSupported("导出已取消");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"SaveAudio failed: {ex}");
            return SaveAudioResult.NotSupported($"导出失败: {ex.Message}");
        }
    }

    private static int GetWaitingSeconds(string word, IWaitingTimeCalculator? calculator)
    {
        if (calculator is not null)
            return Math.Max(0, calculator.CalculateWaitingTime(word));
        return 3;
    }
}
