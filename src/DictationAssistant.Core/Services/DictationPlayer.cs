using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using System.Diagnostics;

namespace DictationAssistant.Core.Services;

public sealed class DictationPlayer : IDictationPlayer
{
    private readonly ITtsEngine? _ttsEngine;
    private readonly IPcmTtsEngine? _pcmTtsEngine;
    private readonly IAudioPlayer _audioPlayer;
    private readonly IWordListSource _wordListSource;
    private readonly object _stateLock = new();

    private CancellationTokenSource? _sessionCancellation;
    private Task? _autoTask;
    private TaskCompletionSource<bool>? _pauseSignal;

    public DictationPlayer(IPcmTtsEngine ttsEngine, IWordListSource wordListSource, IAudioPlayer audioPlayer, DictationSettings? settings = null)
    {
        _pcmTtsEngine = ttsEngine;
        _audioPlayer = audioPlayer;
        _wordListSource = wordListSource;
        Settings = settings ?? new DictationSettings();
        Progress = DictationProgress.Empty with { TotalWords = _wordListSource.Count };

        _wordListSource.Changed += (_, _) =>
        {
            UpdateProgress(progress => progress with
            {
                TotalWords = _wordListSource.Count,
                CurrentWordIndex = Math.Min(progress.CurrentWordIndex, _wordListSource.Count - 1)
            });
        };
    }

    public DictationPlayer(ITtsEngine ttsEngine, IWordListSource wordListSource, IAudioPlayer audioPlayer, DictationSettings? settings = null)
    {
        _ttsEngine = ttsEngine;
        _pcmTtsEngine = ttsEngine as IPcmTtsEngine;
        _audioPlayer = audioPlayer;
        _wordListSource = wordListSource;
        Settings = settings ?? new DictationSettings();
        Progress = DictationProgress.Empty with { TotalWords = _wordListSource.Count };

        _wordListSource.Changed += (_, _) =>
        {
            UpdateProgress(progress => progress with
            {
                TotalWords = _wordListSource.Count,
                CurrentWordIndex = Math.Min(progress.CurrentWordIndex, _wordListSource.Count - 1)
            });
        };
    }

    public DictationPlayer(ITtsEngine ttsEngine, IWordListSource wordListSource, DictationSettings? settings = null)
        : this(ttsEngine, wordListSource, new NoOpAudioPlayer(), settings)
    {
    }

    public DictationState State { get; private set; } = DictationState.Stopped;

    public DictationSettings Settings { get; }

    public DictationProgress Progress { get; private set; }

    public event EventHandler<DictationProgress>? ProgressChanged;

    public event EventHandler<DictationState>? StateChanged;

    public Task SpeakPreviousAsync(CancellationToken cancellationToken = default)
    {
        var target = Math.Max(Progress.CurrentWordIndex - 1, 0);
        return SpeakAtAsync(target, cancellationToken);
    }

    public Task SpeakAgainAsync(CancellationToken cancellationToken = default)
    {
        var target = Progress.CurrentWordIndex >= 0 ? Progress.CurrentWordIndex : 0;
        return SpeakAtAsync(target, cancellationToken);
    }

    public Task SpeakNextAsync(CancellationToken cancellationToken = default)
    {
        var target = Math.Clamp(Progress.CurrentWordIndex + 1, 0, Math.Max(0, _wordListSource.Count - 1));
        return SpeakAtAsync(target, cancellationToken);
    }

    public async Task SpeakAtAsync(int index, CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        index = Math.Clamp(index, 0, _wordListSource.Count - 1);
        await StopAutoInternalAsync().ConfigureAwait(false);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        SetState(DictationState.ManualSpeaking);
        await SpeakWordAsync(index, 1, linkedCts.Token).ConfigureAwait(false);
        SetState(DictationState.Stopped);
    }

    public async Task StartAutoAsync(int startIndex = 0, CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        startIndex = Math.Clamp(startIndex, 0, _wordListSource.Count - 1);
        await StopAutoInternalAsync().ConfigureAwait(false);

        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        lock (_stateLock)
        {
            _sessionCancellation = cts;
            _pauseSignal = null;
        }

        SetState(DictationState.AutoRunning);
        _autoTask = RunAutoAsync(startIndex, cts.Token);
    }

    public void PauseAuto()
    {
        lock (_stateLock)
        {
            if (State != DictationState.AutoRunning)
            {
                return;
            }

            _pauseSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        SetState(DictationState.AutoPaused);
    }

    public void ResumeAuto()
    {
        TaskCompletionSource<bool>? pauseSignal;
        lock (_stateLock)
        {
            if (State != DictationState.AutoPaused)
            {
                return;
            }

            pauseSignal = _pauseSignal;
            _pauseSignal = null;
        }

        pauseSignal?.TrySetResult(true);
        SetState(DictationState.AutoRunning);
    }

    public async Task StopAsync()
    {
        await StopAutoInternalAsync().ConfigureAwait(false);
        SetState(DictationState.Stopped);
    }

    public Task<SaveAudioResult> SaveAudioAsync(SaveAudioRequest request, CancellationToken cancellationToken = default)
    {
        _ = request;
        _ = cancellationToken;
        return Task.FromResult(
            SaveAudioResult.NotSupported("TODO: cross-platform audio export pipeline is scaffolded but not implemented in v4 yet."));
    }

    private async Task RunAutoAsync(int startIndex, CancellationToken cancellationToken)
    {
        try
        {
            for (var index = startIndex; index < _wordListSource.Count; index++)
            {
                for (var repeat = 1; repeat <= Settings.TimesPerWord; repeat++)
                {
                    await WaitIfPausedAsync(cancellationToken).ConfigureAwait(false);
                    await SpeakWordAsync(index, repeat, cancellationToken).ConfigureAwait(false);

                    var shouldDelay = repeat < Settings.TimesPerWord || index < _wordListSource.Count - 1;
                    if (shouldDelay && Settings.IntervalSeconds > 0)
                    {
                        await WaitIfPausedAsync(cancellationToken).ConfigureAwait(false);
                        await Task.Delay(TimeSpan.FromSeconds(Settings.IntervalSeconds), cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            UpdateProgress(progress => progress with { IsCompleted = true });
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            lock (_stateLock)
            {
                _sessionCancellation?.Dispose();
                _sessionCancellation = null;
                _autoTask = null;
                _pauseSignal = null;
            }

            if (State is DictationState.AutoPaused or DictationState.AutoRunning)
            {
                SetState(DictationState.Stopped);
            }
        }
    }

    private async Task WaitIfPausedAsync(CancellationToken cancellationToken)
    {
        TaskCompletionSource<bool>? pauseSignal;
        lock (_stateLock)
        {
            pauseSignal = _pauseSignal;
        }

        if (pauseSignal is null)
        {
            return;
        }

        using var registration = cancellationToken.Register(() => pauseSignal.TrySetCanceled(cancellationToken));
        await pauseSignal.Task.ConfigureAwait(false);
    }

    private async Task SpeakWordAsync(int index, int repeat, CancellationToken cancellationToken)
    {
        var word = _wordListSource.GetWordAt(index);
        UpdateProgress(_ => new DictationProgress(index, repeat, _wordListSource.Count, false));

        try
        {
            var options = new TtsSpeakOptions
            {
                Rate = Settings.Rate,
                VoiceName = ResolveVoiceName(word)
            };

            PcmAudio? pcmAudio;
            if (_pcmTtsEngine is IPreloadableTtsEngine preloadable)
            {
                pcmAudio = await preloadable.TryConsumePreloadedAsync(word, options, cancellationToken).ConfigureAwait(false);
                if (pcmAudio is null)
                {
                    pcmAudio = await _pcmTtsEngine.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);
                }
            }
            else if (_pcmTtsEngine is not null)
            {
                pcmAudio = await _pcmTtsEngine.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);
            }
            else if (_ttsEngine is IConfigurableTtsEngine configurableTtsEngine)
            {
                var wavBytes = await configurableTtsEngine.SynthesizeAudioAsync(word, options, cancellationToken).ConfigureAwait(false);
                pcmAudio = TryDecodeWav(wavBytes);
            }
            else if (_ttsEngine is not null)
            {
                var wavBytes = await _ttsEngine.SynthesizeAudioAsync(word, cancellationToken).ConfigureAwait(false);
                pcmAudio = TryDecodeWav(wavBytes);
            }
            else
            {
                pcmAudio = null;
            }

            if (pcmAudio is null)
            {
                Trace.WriteLine($"TTS synth returned no playable PCM for word '{word}'.");
                return;
            }

            await _audioPlayer.PlayAsync(pcmAudio, Settings.Volume, cancellationToken).ConfigureAwait(false);

            if (_pcmTtsEngine is IPreloadableTtsEngine preloadable2 && index + 1 < _wordListSource.Count)
            {
                var nextWord = _wordListSource.GetWordAt(index + 1);
                var nextOptions = new TtsSpeakOptions
                {
                    Rate = Settings.Rate,
                    VoiceName = ResolveVoiceName(nextWord)
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await preloadable2.PreloadAsync(nextWord, nextOptions, CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Preload failed for '{nextWord}': {ex}");
                    }
                });
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"TTS speak failed for word '{word}': {ex}");
        }
    }

    private static PcmAudio? TryDecodeWav(byte[]? wavBytes)
    {
        if (wavBytes is null || wavBytes.Length == 0)
        {
            return null;
        }

        if (!WavReader.TryReadPcmAudio(wavBytes, out var pcmAudio, out var error))
        {
            Trace.WriteLine($"Skipping non-WAV or unsupported audio payload: {error}");
            return null;
        }

        return pcmAudio;
    }

    private string? ResolveVoiceName(string text)
    {
        if (ContainsAsciiLetter(text) && !string.IsNullOrWhiteSpace(Settings.DefaultEnglishVoiceName))
        {
            return Settings.DefaultEnglishVoiceName;
        }

        if (!string.IsNullOrWhiteSpace(Settings.DefaultChineseVoiceName))
        {
            return Settings.DefaultChineseVoiceName;
        }

        return null;
    }

    private static bool ContainsAsciiLetter(string text)
    {
        foreach (var ch in text)
        {
            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
            {
                return true;
            }
        }

        return false;
    }

    private async Task StopAutoInternalAsync()
    {
        Task? currentAutoTask;
        CancellationTokenSource? cts;

        lock (_stateLock)
        {
            currentAutoTask = _autoTask;
            cts = _sessionCancellation;
            _pauseSignal?.TrySetCanceled();
            _pauseSignal = null;
            _autoTask = null;
            _sessionCancellation = null;
        }

        cts?.Cancel();

        if (currentAutoTask is not null)
        {
            try
            {
                await currentAutoTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
        }

        cts?.Dispose();
    }

    private void SetState(DictationState state)
    {
        if (State == state)
        {
            return;
        }

        State = state;
        StateChanged?.Invoke(this, state);
    }

    private void UpdateProgress(Func<DictationProgress, DictationProgress> mutate)
    {
        Progress = mutate(Progress);
        ProgressChanged?.Invoke(this, Progress);
    }

    private sealed class NoOpAudioPlayer : IAudioPlayer
    {
        public Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct)
        {
            _ = audio;
            _ = volume;
            _ = ct;
            return Task.CompletedTask;
        }
    }
}
