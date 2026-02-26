using DictationAssistant.App.Models;

namespace DictationAssistant.App.Abstractions;

public interface IDictationPlayer
{
    DictationState State { get; }

    DictationSettings Settings { get; }

    DictationProgress Progress { get; }

    IVoice Voice { get; set; }

    IWaitingTimeCalculator? WaitingTimeCalculator { get; set; }

    event EventHandler<DictationProgress>? ProgressChanged;

    event EventHandler<DictationState>? StateChanged;

    Task SpeakPreviousAsync(CancellationToken cancellationToken = default);

    Task SpeakAgainAsync(CancellationToken cancellationToken = default);

    Task SpeakNextAsync(CancellationToken cancellationToken = default);

    Task SpeakAtAsync(int index, CancellationToken cancellationToken = default);

    Task StartAutoAsync(int startIndex = 0, CancellationToken cancellationToken = default);

    void PauseAuto();

    void ResumeAuto();

    Task StopAsync();

    Task<SaveAudioResult> SaveAudioAsync(SaveAudioRequest request, CancellationToken cancellationToken = default);
}
