using DictationAssistant.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.Audio;
using DictationAssistant.Models;
using System.Diagnostics;

namespace DictationAssistant.Services;

public sealed class AudioExporter
{
    private readonly IVoice _voice;
    private readonly IWordListSource _wordListSource;
    private readonly IWaitingTimeCalculator? _waitingTimeCalculator;
    private readonly DictationSettings _settings;

    public AudioExporter(
        IVoice voice,
        IWordListSource wordListSource,
        IWaitingTimeCalculator? waitingTimeCalculator = null,
        DictationSettings? settings = null)
    {
        _voice = voice;
        _wordListSource = wordListSource;
        _waitingTimeCalculator = waitingTimeCalculator;
        _settings = settings ?? new DictationSettings();
    }

    public async Task<SaveAudioResult> ExportAudioAsync(
        SaveAudioRequest request,
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

            var totalWords = _wordListSource.Count;
            var totalSegments = totalWords * _settings.TimesPerWord;
            var currentSegment = 0;
            var accumulatedDuration = TimeSpan.Zero;

            if (generateLrc)
            {
                lyricWriter = new LyricWriter(request.LyricsOutputPath!);
            }

            for (var index = 0; index < totalWords; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var word = _wordListSource.GetWordAt(index);

                for (var repeat = 1; repeat <= _settings.TimesPerWord; repeat++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var options = new VoiceSynthesisOptions
                    {
                        Rate = _settings.Rate
                    };

                    var pcmAudio = await _voice.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);

                    if (pcmAudio is not null)
                    {
                        if (generateLrc && repeat == 1)
                        {
                            lyricWriter?.WriteTimestamp((long)accumulatedDuration.TotalMilliseconds, word);
                        }

                        await pcmStream.WriteAsync(pcmAudio.ToArray(), cancellationToken).ConfigureAwait(false);

                        var audioDuration = TimeSpan.FromSeconds(
                            (double)pcmAudio.Data.Length / (pcmAudio.Format.SampleRate * pcmAudio.Format.Channels * 2));
                        accumulatedDuration += audioDuration;
                    }

                    if (repeat < _settings.TimesPerWord || index < totalWords - 1)
                    {
                        var silenceDuration = GetWaitingSeconds(word);
                        if (silenceDuration > 0 && pcmAudio is not null)
                        {
                            var silenceBytes = CreateSilencePcm(silenceDuration, pcmAudio.Format.SampleRate, pcmAudio.Format.Channels);
                            await pcmStream.WriteAsync(silenceBytes, cancellationToken).ConfigureAwait(false);
                            accumulatedDuration += TimeSpan.FromSeconds(silenceDuration);
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
            var sampleRate = request.SampleRate;
            var channels = request.Channels;

            if (pcmData.Length > 0)
            {
                sampleRate = 44100;
                channels = 2;
            }

            if (format == "wav")
            {
                await using var outputStream = new FileStream(request.OutputPath, FileMode.Create, FileAccess.Write);
                WavWriter.WriteHeader(outputStream, sampleRate, channels, pcmData.Length);
                await outputStream.WriteAsync(pcmData, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await using var encoder = new FFmpegAudioEncoder(request.OutputPath, sampleRate, channels, format);
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

    private int GetWaitingSeconds(string word)
    {
        if (_waitingTimeCalculator is not null)
            return Math.Max(0, _waitingTimeCalculator.CalculateWaitingTime(word));
        if (int.TryParse(_settings.IntervalExpression, out var secs))
            return Math.Clamp(secs, 0, 600);
        return 3;
    }

    private static byte[] CreateSilencePcm(double durationSeconds, int sampleRate, int channels)
    {
        var bytesPerSample = 2;
        var totalBytes = (int)(sampleRate * channels * bytesPerSample * durationSeconds);
        return new byte[totalBytes];
    }
}
