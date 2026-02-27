using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;

namespace DictationAssistant.App.Services;

public sealed class AudioExporter
{
    private readonly IVoice _voice;
    private readonly IWordListSource _wordListSource;
    private readonly IWaitingTimeCalculator? _waitingTimeCalculator;

    public AudioExporter(IVoice voice, IWordListSource wordListSource, IWaitingTimeCalculator? waitingTimeCalculator = null)
    {
        _voice = voice;
        _wordListSource = wordListSource;
        _waitingTimeCalculator = waitingTimeCalculator;
    }

    public async Task ExportAsync(
        PcmWriter pcmWriter,
        ILyricWriter? lyricWriter,
        int timesPerWord,
        int volume,
        int rate,
        IProgress<double>? progress,
        CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            throw new InvalidOperationException("没有词语可导出");
        }

        var totalWords = _wordListSource.Count;
        var totalSegments = totalWords * timesPerWord;
        var currentSegment = 0;
        long byteOffset = 0;

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
                    if (lyricWriter is not null && repeat == 1)
                    {
                        lyricWriter.WriteTimestamp(pcmWriter.BytesToMilliseconds(byteOffset), word);
                    }

                    byteOffset += pcmWriter.Write(pcmAudio);
                }

                if (repeat < timesPerWord || index < totalWords - 1)
                {
                    var silenceMs = GetWaitingSeconds(word) * 1000;
                    if (silenceMs > 0 && pcmAudio is not null)
                    {
                        byteOffset += pcmWriter.WriteDelay(silenceMs);
                    }
                }

                currentSegment++;
                progress?.Report((double)currentSegment / totalSegments);
            }
        }

        if (lyricWriter is not null)
        {
            lyricWriter.Flush();
        }

        progress?.Report(1.0);
    }

    private int GetWaitingSeconds(string word)
    {
        if (_waitingTimeCalculator is not null)
            return Math.Max(0, _waitingTimeCalculator.CalculateWaitingTime(word));
        return 3;
    }
}
