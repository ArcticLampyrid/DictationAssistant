using DictationAssistant.App.Models;
using DictationAssistant.App.Lyric;
using DictationAssistant.App.Audio;

namespace DictationAssistant.App.Abstractions;

public interface IDictationPlayer
{
    bool AutoMode { get; set; }
    bool IsSpeaking { get; }
    bool IsPaused { get; }

    IVoice Voice { get; set; }
    IWaitingTimeCalculator? WaitingTimeCalculator { get; set; }
    int TimesPerWord { get; set; }
    int Volume { get; set; }
    int Rate { get; set; }

    DictationProgress Progress { get; }

    event EventHandler<DictationProgress>? ProgressChanged;

    void SpeakAt(int index);
    void SpeakNext();
    void SpeakPrevious();
    void SpeakAgain();

    void StartAuto(int startIndex = 0);
    void PauseAuto();
    void ResumeAuto();
    void Stop();
    void ResetProgress();

    Task ExportAudioAsync(PcmWriter pcmWriter, ILyricWriter? lyricWriter, IProgress<double>? progress, CancellationToken cancellationToken = default);
}
