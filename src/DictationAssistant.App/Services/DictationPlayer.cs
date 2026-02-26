using DictationAssistant.Abstractions;
using DictationAssistant.Audio;
using DictationAssistant.Models;
using System.Diagnostics;

namespace DictationAssistant.Services;

public sealed class DictationPlayer : IDictationPlayer
{
    private IVoice _voice;
    private readonly IAudioPlayer _audioPlayer;
    private readonly IWordListSource _wordListSource;
    private readonly object _stateLock = new();
    private IWaitingTimeCalculator? _waitingTimeCalculator;

    private CancellationTokenSource? _sessionCancellation;
    private Task? _autoTask;
    private TaskCompletionSource<bool>? _pauseSignal;

    public DictationPlayer(IVoice voice, IWordListSource wordListSource, IAudioPlayer audioPlayer, DictationSettings? settings = null)
    {
        _voice = voice;
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

    private int GetWaitingSeconds(string word)
    {
        if (_waitingTimeCalculator is not null)
            return Math.Max(0, _waitingTimeCalculator.CalculateWaitingTime(word));
        if (int.TryParse(Settings.IntervalExpression, out var secs))
            return Math.Clamp(secs, 0, 600);
        return 3;
    }

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

    public async Task<SaveAudioResult> SaveAudioAsync(SaveAudioRequest request, CancellationToken cancellationToken = default)
    {
        return await SaveAudioInternalAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<SaveAudioResult> SaveAudioInternalAsync(SaveAudioRequest request, CancellationToken cancellationToken)
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

        try
        {
            await using var pcmStream = new MemoryStream();

            var totalWords = _wordListSource.Count;
            var totalSegments = totalWords * Settings.TimesPerWord;
            var currentSegment = 0;
            var lrcLines = new List<string>();
            var accumulatedDuration = TimeSpan.Zero;

            for (var index = 0; index < totalWords; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var word = _wordListSource.GetWordAt(index);

                for (var repeat = 1; repeat <= Settings.TimesPerWord; repeat++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var options = new VoiceSynthesisOptions
                    {
                        Rate = Settings.Rate
                    };

                    var pcmAudio = await _voice.SynthesizePcmAsync(word, options, cancellationToken).ConfigureAwait(false);

                    if (pcmAudio is not null)
                    {
                        if (generateLrc && repeat == 1)
                        {
                            var lrcTime = FormatLrcTime(accumulatedDuration);
                            lrcLines.Add($"[{lrcTime}]{word}");
                        }

                        await pcmStream.WriteAsync(pcmAudio.ToArray(), cancellationToken).ConfigureAwait(false);

                        var audioDuration = TimeSpan.FromSeconds(
                            (double)pcmAudio.Data.Length / (pcmAudio.Format.SampleRate * pcmAudio.Format.Channels * 2));
                        accumulatedDuration += audioDuration;
                    }

                    if (repeat < Settings.TimesPerWord || index < totalWords - 1)
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

            if (generateLrc && lrcLines.Count > 0)
            {
                await File.WriteAllTextAsync(request.LyricsOutputPath!, string.Join(Environment.NewLine, lrcLines), cancellationToken).ConfigureAwait(false);
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

    private static byte[] CreateSilencePcm(double durationSeconds, int sampleRate, int channels)
    {
        var bytesPerSample = 2;
        var totalBytes = (int)(sampleRate * channels * bytesPerSample * durationSeconds);
        return new byte[totalBytes];
    }

    private static string FormatLrcTime(TimeSpan time)
    {
        var minutes = (int)time.TotalMinutes;
        var seconds = time.Seconds;
        var hundredths = time.Milliseconds / 10;
        return $"{minutes:D2}:{seconds:D2}.{hundredths:D2}";
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
                    if (shouldDelay)
                    {
                        var word = _wordListSource.GetWordAt(index);
                        var waitSeconds = GetWaitingSeconds(word);
                        if (waitSeconds > 0)
                        {
                            await WaitIfPausedAsync(cancellationToken).ConfigureAwait(false);
                            await Task.Delay(TimeSpan.FromSeconds(waitSeconds), cancellationToken).ConfigureAwait(false);
                        }
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
