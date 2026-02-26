using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;
using System.Diagnostics;

namespace DictationAssistant.App.Services;

public sealed class DictationPlayer : IDictationPlayer
{
    private IVoice _voice;
    private readonly IAudioPlayer _audioPlayer;
    private readonly IWordListSource _wordListSource;
    private readonly object _stateLock = new();
    private IWaitingTimeCalculator? _waitingTimeCalculator;
    private readonly AudioExporter _audioExporter;

    // Timer CTS controls the auto-play wait loop. Cancelled on pause/stop/manual speak.
    private CancellationTokenSource? _timerCts;
    private Task? _autoTimerTask;

    // Playback CTS controls the current SpeakWordAsync call. Cancelled when we need to
    // interrupt playback (e.g. user clicks SpeakNext while currently speaking).
    private CancellationTokenSource? _playbackCts;

    private bool _autoMode;
    private bool _isPausedAutoMode;
    private int _elapsedTimes;

    public DictationPlayer(IVoice voice, IWordListSource wordListSource, IAudioPlayer audioPlayer, DictationSettings? settings = null)
    {
        _voice = voice;
        _audioPlayer = audioPlayer;
        _wordListSource = wordListSource;
        Settings = settings ?? new DictationSettings();
        Progress = DictationProgress.Empty with { TotalWords = _wordListSource.Count };
        _audioExporter = new AudioExporter(voice, wordListSource, null, Settings);

        _wordListSource.Changed += (_, _) =>
        {
            UpdateProgress(progress => progress with
            {
                TotalWords = _wordListSource.Count,
                CurrentWordIndex = Math.Min(progress.CurrentWordIndex, _wordListSource.Count - 1)
            });
        };
    }

    public DictationState State { get; private set; } = DictationState.Stopped;

    public DictationSettings Settings { get; }

    public IWaitingTimeCalculator? WaitingTimeCalculator
    {
        get => _waitingTimeCalculator;
        set => _waitingTimeCalculator = value;
    }

    public IVoice Voice
    {
        get => _voice;
        set => _voice = value;
    }

    private TimeSpan GetWaitingTime(string word)
    {
        if (_waitingTimeCalculator is not null)
        {
            var secs = _waitingTimeCalculator.CalculateWaitingTime(word);
            return TimeSpan.FromSeconds(Math.Max(0, secs));
        }
        if (int.TryParse(Settings.IntervalExpression, out var parsed))
            return TimeSpan.FromSeconds(Math.Clamp(parsed, 0, 600));
        return TimeSpan.FromSeconds(3);
    }

    public DictationProgress Progress { get; private set; }

    public event EventHandler<DictationProgress>? ProgressChanged;

    public event EventHandler<DictationState>? StateChanged;

    /// <summary>
    /// Stops auto-play and resets position to -1 (before first word).
    /// Equivalent to v3.x ResetProgress + StopAuto.
    /// </summary>
    public void ResetProgress()
    {
        CancelTimerAndPlayback();

        lock (_stateLock)
        {
            _autoMode = false;
            _isPausedAutoMode = false;
            _elapsedTimes = 0;
        }

        UpdateProgress(_ => new DictationProgress(-1, 0, _wordListSource.Count, false));
        SetState(DictationState.Stopped);
    }

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

    /// <summary>
    /// Speaks a specific word by index. Matches v3.x Speak(index):
    /// - Stops timer and current playback
    /// - Resets ElapsedTimes if position changed
    /// - Clears pause flag (so auto resumes after this word)
    /// - Plays the word
    /// - After play completes, if AutoMode → restart wait loop
    /// </summary>
    public async Task SpeakAtAsync(int index, CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        index = Math.Clamp(index, 0, _wordListSource.Count - 1);

        bool wasAutoMode;
        bool positionChanged;

        // Cancel timer and any current playback
        CancelTimerAndPlayback();

        lock (_stateLock)
        {
            wasAutoMode = _autoMode;
            positionChanged = Progress.CurrentWordIndex != index;

            if (_isPausedAutoMode)
            {
                // Manual speak during pause resumes auto mode (v3.x behavior)
                _isPausedAutoMode = false;
            }

            if (positionChanged)
            {
                _elapsedTimes = 0;
            }
        }

        // Wait for timer task to finish (it should exit quickly after cancellation)
        await AwaitTimerTaskAsync().ConfigureAwait(false);

        var effectiveState = wasAutoMode ? DictationState.AutoRunning : DictationState.ManualSpeaking;
        SetState(effectiveState);

        // Create a new playback CTS for this speak operation
        var playbackCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        lock (_stateLock)
        {
            _playbackCts = playbackCts;
        }

        try
        {
            await SpeakWordAsync(index, playbackCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Playback was interrupted (e.g. another SpeakAt call)
            return;
        }
        finally
        {
            lock (_stateLock)
            {
                if (_playbackCts == playbackCts)
                {
                    _playbackCts = null;
                }
            }
            playbackCts.Dispose();
        }

        // After play completes: if auto mode, restart wait loop (v3.x: PlayCompleted → timer)
        bool autoMode;
        lock (_stateLock)
        {
            autoMode = _autoMode;
            if (autoMode)
            {
                _elapsedTimes += 1;
            }
        }

        if (autoMode)
        {
            SetState(DictationState.AutoRunning);
            StartAutoWaitLoop();
        }
        else
        {
            SetState(DictationState.Stopped);
        }
    }

    /// <summary>
    /// Starts auto-play from the specified index. Always from index 0 when called from UI.
    /// </summary>
    public async Task StartAutoAsync(int startIndex = 0, CancellationToken cancellationToken = default)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        startIndex = Math.Clamp(startIndex, 0, _wordListSource.Count - 1);

        CancelTimerAndPlayback();

        lock (_stateLock)
        {
            _isPausedAutoMode = false;
            _autoMode = true;
            _elapsedTimes = 0;
        }

        await AwaitTimerTaskAsync().ConfigureAwait(false);

        SetState(DictationState.AutoRunning);

        // Create playback CTS for the initial speak
        var playbackCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        lock (_stateLock)
        {
            _playbackCts = playbackCts;
        }

        try
        {
            await SpeakWordAsync(startIndex, playbackCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        finally
        {
            lock (_stateLock)
            {
                if (_playbackCts == playbackCts)
                {
                    _playbackCts = null;
                }
            }
            playbackCts.Dispose();
        }

        bool autoMode;
        lock (_stateLock)
        {
            autoMode = _autoMode;
            if (autoMode)
            {
                _elapsedTimes = 1;
            }
        }

        if (autoMode)
        {
            StartAutoWaitLoop();
        }
    }

    /// <summary>
    /// Pauses auto-play. Keeps AutoMode=true, sets IsPausedAutoMode=true.
    /// Cancels current timer and playback.
    /// </summary>
    public void PauseAuto()
    {
        lock (_stateLock)
        {
            if (!_autoMode)
            {
                return;
            }

            _isPausedAutoMode = true;
        }

        CancelTimerAndPlayback();
        SetState(DictationState.AutoPaused);
    }

    /// <summary>
    /// Resumes auto-play from where it was paused. Restarts the wait loop.
    /// </summary>
    public void ResumeAuto()
    {
        lock (_stateLock)
        {
            if (!_isPausedAutoMode)
            {
                return;
            }

            _isPausedAutoMode = false;
        }

        SetState(DictationState.AutoRunning);
        StartAutoWaitLoop();
    }

    /// <summary>
    /// Stops auto-play but keeps current position. User can still use manual speak.
    /// </summary>
    public Task StopAsync()
    {
        CancelTimerAndPlayback();

        lock (_stateLock)
        {
            _autoMode = false;
            _isPausedAutoMode = false;
        }

        SetState(DictationState.Stopped);
        return Task.CompletedTask;
    }

    public async Task<SaveAudioResult> SaveAudioAsync(SaveAudioRequest request, CancellationToken cancellationToken = default)
    {
        return await _audioExporter.ExportAudioAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private void CancelTimerAndPlayback()
    {
        _timerCts?.Cancel();
        _timerCts?.Dispose();
        _timerCts = null;

        _playbackCts?.Cancel();
        // Don't dispose _playbackCts here — SpeakAtAsync/StartAutoAsync owns it via finally block
    }

    private async Task AwaitTimerTaskAsync()
    {
        if (_autoTimerTask is { } timerTask)
        {
            try
            {
                await timerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            _autoTimerTask = null;
        }
    }

    private void StartAutoWaitLoop()
    {
        var cts = new CancellationTokenSource();
        _timerCts = cts;

        _autoTimerTask = Task.Run(async () =>
        {
            try
            {
                await AutoWaitLoopAsync(cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
        });
    }

    /// <summary>
    /// Auto-play wait loop. After play completes, waits the configured interval
    /// for the current word, then speaks the next word (or repeats current).
    /// Uses a single Task.Delay for the full wait duration instead of polling.
    /// </summary>
    private async Task AutoWaitLoopAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int currentWordIndex;
            int elapsedTimes;

            lock (_stateLock)
            {
                if (!_autoMode || _isPausedAutoMode)
                    return;
                currentWordIndex = Progress.CurrentWordIndex;
                elapsedTimes = _elapsedTimes;
            }

            if (currentWordIndex < 0 || currentWordIndex >= _wordListSource.Count)
                return;

            // Calculate and wait the full interval in one go
            var word = _wordListSource.GetWordAt(currentWordIndex);
            var waitingTime = GetWaitingTime(word);

            if (waitingTime > TimeSpan.Zero)
            {
                await Task.Delay(waitingTime, cancellationToken).ConfigureAwait(false);
            }

            // Re-check state after waiting (may have been paused/stopped during the wait)
            lock (_stateLock)
            {
                if (!_autoMode || _isPausedAutoMode)
                    return;
                // Re-read in case manual speak happened during wait
                currentWordIndex = Progress.CurrentWordIndex;
                elapsedTimes = _elapsedTimes;
            }

            // Decide what to speak next
            int speakIndex;
            if (elapsedTimes >= Settings.TimesPerWord)
            {
                speakIndex = currentWordIndex + 1; // advance to next word
            }
            else
            {
                speakIndex = currentWordIndex; // repeat current word
            }

            // Check if we've reached the end
            if (speakIndex >= _wordListSource.Count)
            {
                lock (_stateLock)
                {
                    _autoMode = false;
                }
                UpdateProgress(p => p with { IsCompleted = true });
                SetState(DictationState.Stopped);
                return;
            }

            // Reset counters for the new speak
            lock (_stateLock)
            {
                if (speakIndex != currentWordIndex)
                    _elapsedTimes = 0; // new word
            }

            // Speak the word
            using var playbackCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            lock (_stateLock)
            {
                _playbackCts = playbackCts;
            }

            try
            {
                await SpeakWordAsync(speakIndex, playbackCts.Token).ConfigureAwait(false);
            }
            finally
            {
                lock (_stateLock)
                {
                    if (_playbackCts == playbackCts)
                    {
                        _playbackCts = null;
                    }
                }
            }

            // After play completes, increment times and loop back to wait
            lock (_stateLock)
            {
                if (!_autoMode) return;
                _elapsedTimes += 1;
            }
        }
    }

    private async Task SpeakWordAsync(int index, CancellationToken cancellationToken)
    {
        if (index < 0 || index >= _wordListSource.Count)
        {
            return;
        }

        var word = _wordListSource.GetWordAt(index);

        int currentRepeat;
        lock (_stateLock)
        {
            currentRepeat = _elapsedTimes;
        }

        UpdateProgress(_ => new DictationProgress(index, currentRepeat, _wordListSource.Count, false));

        try
        {
            var options = new VoiceSynthesisOptions
            {
                Rate = Settings.Rate
            };

            var pcmAudio = await _voice.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);

            if (pcmAudio is null)
            {
                Trace.WriteLine($"TTS synth returned no playable PCM for word '{word}'.");
                return;
            }

            await _audioPlayer.PlayAsync(pcmAudio, Settings.Volume, cancellationToken).ConfigureAwait(false);

            if (_voice is IPreloadableVoice preloadable && index + 1 < _wordListSource.Count)
            {
                var nextWord = _wordListSource.GetWordAt(index + 1);
                var nextOptions = new VoiceSynthesisOptions
                {
                    Rate = Settings.Rate
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await preloadable.PreloadAsync(nextWord, nextOptions, CancellationToken.None).ConfigureAwait(false);
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
}
