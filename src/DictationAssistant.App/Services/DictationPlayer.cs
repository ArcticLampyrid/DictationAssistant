using DictationAssistant.App.Abstractions;
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
    private Task? _currentChainTask;
    private CancellationTokenSource? _currentChainCts;
    private bool _autoMode;
    private bool _isPaused;
    private int _elapsedTimes;

    public int TimesPerWord { get; set; } = 2;
    public int Volume { get => _audioPlayer.Volume; set => _audioPlayer.Volume = value; }
    public int Rate { get; set; } = 0;

    public DictationPlayer(IVoice voice, IWordListSource wordListSource, IAudioPlayer audioPlayer)
    {
        _voice = voice;
        _audioPlayer = audioPlayer;
        _wordListSource = wordListSource;
        Progress = DictationProgress.Empty with { TotalWords = _wordListSource.Count };
    }

    public bool AutoMode
    {
        get => _autoMode;
        set
        {
            lock (_stateLock)
            {
                _autoMode = value;
            }
        }
    }

    public bool IsSpeaking { get; private set; }

    public bool IsPaused
    {
        get
        {
            lock (_stateLock)
            {
                return _autoMode && _isPaused;
            }
        }
    }

    public IWaitingTimeCalculator? WaitingTimeCalculator
    {
        get => _waitingTimeCalculator;
        set => _waitingTimeCalculator = value;
    }

    public IWordListSource WordListSource => _wordListSource;

    public IVoice Voice
    {
        get => _voice;
        set => _voice = value;
    }

    public DictationProgress Progress { get; private set; }

    public event EventHandler<DictationProgress>? ProgressChanged;

    private TimeSpan GetWaitingTime(string word)
    {
        if (_waitingTimeCalculator is not null)
        {
            var secs = _waitingTimeCalculator.CalculateWaitingTime(word);
            return TimeSpan.FromSeconds(Math.Max(0, secs));
        }
        return TimeSpan.FromSeconds(3);
    }

    public void ResetProgress()
    {
        CancelChain().ContinueWith(_ =>
        {
            lock (_stateLock)
            {
                _autoMode = false;
                _isPaused = false;
                _elapsedTimes = 0;
            }

            UpdateProgress(_ => new DictationProgress(-1, 0, _wordListSource.Count, null, null, false));
        });
    }

    public void SpeakNext()
    {
        var target = Math.Clamp(Progress.CurrentWordIndex + 1, 0, Math.Max(0, _wordListSource.Count - 1));
        SpeakAt(target);
    }

    public void SpeakPrevious()
    {
        var target = Math.Max(Progress.CurrentWordIndex - 1, 0);
        SpeakAt(target);
    }

    public void SpeakAgain()
    {
        var target = Progress.CurrentWordIndex >= 0 ? Progress.CurrentWordIndex : 0;
        SpeakAt(target);
    }

    public void SpeakAt(int index, bool resetElapsedTimes = false)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        index = Math.Clamp(index, 0, _wordListSource.Count - 1);

        CancelChain().ContinueWith(_ =>
        {
            lock (_stateLock)
            {
                _isPaused = false;
                bool positionChanged = Progress.CurrentWordIndex != index;
                if (resetElapsedTimes || positionChanged)
                {
                    _elapsedTimes = 0;
                }
            }

            var cts = new CancellationTokenSource();
            _currentChainCts = cts;
            _currentChainTask = SpeakChainAsync(index, cts.Token);
        });
    }

    public void StartAuto(int startIndex = 0)
    {
        if (_wordListSource.Count <= 0)
        {
            return;
        }

        lock (_stateLock)
        {
            _autoMode = true;
            _isPaused = false;
        }

        var target = Math.Clamp(startIndex, 0, _wordListSource.Count - 1);
        SpeakAt(target, true);
    }

    public void PauseAuto()
    {
        lock (_stateLock)
        {
            if (!_autoMode)
            {
                return;
            }

            _isPaused = true;
        }

        _ = CancelChain();
    }

    public void ResumeAuto()
    {
        lock (_stateLock)
        {
            if (!_isPaused)
            {
                return;
            }

            _isPaused = false;
        }

        var cts = new CancellationTokenSource();
        _currentChainCts = cts;
        _currentChainTask = ScheduleAndContinueAsync(Progress.CurrentWordIndex, cts.Token);
    }

    public void Stop()
    {
        lock (_stateLock)
        {
            _autoMode = false;
            _isPaused = false;
        }

        CancelChain().ContinueWith(_ =>
        {
            UpdateProgress(p => p with { NextWordIndex = null, NextSpeakTime = null });
        });
    }


    private async Task CancelChain()
    {
        _currentChainCts?.Cancel();
        _currentChainCts?.Dispose();
        _currentChainCts = null;
        if (_currentChainTask is not null)
        {
            try
            {
                await _currentChainTask;
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            _currentChainTask.Dispose();
            _currentChainTask = null;
        }
    }

    private async Task SpeakChainAsync(int index, CancellationToken ct)
    {
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            int currentElapsedTimes;
            lock (_stateLock)
            {
                currentElapsedTimes = _elapsedTimes;
            }

            IsSpeaking = true;
            UpdateProgress(_ => new DictationProgress(index, currentElapsedTimes, _wordListSource.Count, null, null, false));

            await SpeakWordAsync(index, ct).ConfigureAwait(false);

            IsSpeaking = false;

            lock (_stateLock)
            {
                if (!_autoMode || _isPaused)
                {
                    UpdateProgress(p => p with { NextWordIndex = null, NextSpeakTime = null });
                    return;
                }

                _elapsedTimes += 1;
                currentElapsedTimes = _elapsedTimes;
            }

            int nextIndex;
            if (currentElapsedTimes >= TimesPerWord)
            {
                nextIndex = index + 1;
            }
            else
            {
                nextIndex = index;
            }

            var word = _wordListSource.TryGetAt(index);
            if (word is null)
            {
                lock (_stateLock)
                {
                    _autoMode = false;
                }
                UpdateProgress(p => p with { IsCompleted = true, NextWordIndex = null, NextSpeakTime = null });
                return;
            }

            var waitDuration = GetWaitingTime(word);
            var scheduledTime = DateTimeOffset.Now + waitDuration;

            UpdateProgress(_ => new DictationProgress(index, currentElapsedTimes, _wordListSource.Count, nextIndex, scheduledTime, false));

            if (waitDuration > TimeSpan.Zero)
            {
                await Task.Delay(waitDuration, ct).ConfigureAwait(false);
            }

            lock (_stateLock)
            {
                if (!_autoMode || _isPaused)
                {
                    UpdateProgress(p => p with { NextWordIndex = null, NextSpeakTime = null });
                    return;
                }
            }

            if (nextIndex != index)
            {
                lock (_stateLock)
                {
                    _elapsedTimes = 0;
                }
            }

            index = nextIndex;
        }
    }

    private async Task ScheduleAndContinueAsync(int currentIndex, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        int currentElapsedTimes;
        lock (_stateLock)
        {
            currentElapsedTimes = _elapsedTimes;
        }

        int nextIndex;
        if (currentElapsedTimes >= TimesPerWord)
        {
            nextIndex = currentIndex + 1;
        }
        else
        {
            nextIndex = currentIndex;
        }

        var word = _wordListSource.TryGetAt(currentIndex);
        if (word is null)
        {
            lock (_stateLock)
            {
                _autoMode = false;
            }
            UpdateProgress(p => p with { IsCompleted = true, NextWordIndex = null, NextSpeakTime = null });
            return;
        }

        var waitDuration = GetWaitingTime(word);
        var scheduledTime = DateTimeOffset.Now + waitDuration;

        UpdateProgress(_ => new DictationProgress(currentIndex, currentElapsedTimes, _wordListSource.Count, nextIndex, scheduledTime, false));

        if (waitDuration > TimeSpan.Zero)
        {
            await Task.Delay(waitDuration, ct).ConfigureAwait(false);
        }

        lock (_stateLock)
        {
            if (!_autoMode || _isPaused)
            {
                UpdateProgress(p => p with { NextWordIndex = null, NextSpeakTime = null });
                return;
            }
        }

        if (nextIndex != currentIndex)
        {
            lock (_stateLock)
            {
                _elapsedTimes = 0;
            }
        }

        await SpeakChainAsync(nextIndex, ct).ConfigureAwait(false);
    }

    private async Task SpeakWordAsync(int index, CancellationToken ct)
    {
        if (index < 0 || index >= _wordListSource.Count)
        {
            return;
        }

        var word = _wordListSource.TryGetAt(index);

        var options = new VoiceSynthesisOptions
        {
            Rate = Rate
        };

        var pcmAudio = word is null ? PcmAudio.Empty : await _voice.SynthesizePcmAsync(word, options, ct).ConfigureAwait(false);

        // Preload the next word while the current one is being played, 
        // if supported by the voice and if there is a next word.
        if (_voice is IPreloadableVoice preloadable)
        {
            var nextWord = _wordListSource.TryGetAt(index + 1);
            var nextOptions = new VoiceSynthesisOptions
            {
                Rate = Rate
            };

            if (nextWord is not null)
            {
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
                }, CancellationToken.None);
            }
        }

        await _audioPlayer.PlayAsync(pcmAudio, ct).ConfigureAwait(false);
    }

    private void UpdateProgress(Func<DictationProgress, DictationProgress> mutate)
    {
        Progress = mutate(Progress);
        ProgressChanged?.Invoke(this, Progress);
    }
}
