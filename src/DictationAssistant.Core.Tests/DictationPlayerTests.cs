using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;
using Xunit;

namespace DictationAssistant.Core.Tests;

public static class TaskExtensions
{
    public static async Task WaitForAutoCompleteAsync(this Task task)
    {
        try
        {
            await task;
        }
        catch { }
    }
}

public class DictationPlayerTests
{
    private sealed class FakePcmTtsEngine : IPcmTtsEngine
    {
        public string Name => "Fake";

        public Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken ct)
        {
            _ = ct;
            return Task.FromResult<IReadOnlyList<TtsVoiceInfo>>([]);
        }

        public Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
        {
            _ = text;
            _ = options;
            _ = ct;
            return Task.FromResult<PcmAudio?>(new PcmAudio
            {
                Data = [0, 0],
                Format = new PcmFormatInfo(16000, 1, PcmSampleFormat.S16LE)
            });
        }
    }

    private sealed class FakeAudioPlayer : IAudioPlayer
    {
        public Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct)
        {
            _ = audio;
            _ = volume;
            _ = ct;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public void InitialState_IsStopped_ProgressEmpty()
    {
        var wordList = new WordListDocument("apple\nbanana\ncherry");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer());

        Assert.Equal(DictationState.Stopped, player.State);
        Assert.Equal(-1, player.Progress.CurrentWordIndex);
        Assert.Equal(0, player.Progress.CurrentRepeat);
        Assert.Equal(3, player.Progress.TotalWords);
    }

    [Fact]
    public async Task SpeakAtAsync_StateTransitions_StoppedToManualSpeakingToStopped()
    {
        var wordList = new WordListDocument("apple\nbanana\ncherry");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer());

        var stateChanges = new List<DictationState>();
        player.StateChanged += (_, state) => stateChanges.Add(state);

        await player.SpeakAtAsync(1);

        Assert.Equal(DictationState.Stopped, player.State);
        Assert.Contains(DictationState.ManualSpeaking, stateChanges);
    }

    [Fact]
    public async Task StartAutoAsync_RunAutoAsync_StateStoppedToAutoRunningToStopped()
    {
        var wordList = new WordListDocument("a\nb");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer())
        {
            Settings = { IntervalExpression = "0", TimesPerWord = 1 }
        };

        var stateChanges = new List<DictationState>();
        player.StateChanged += (_, state) => stateChanges.Add(state);

        await player.StartAutoAsync();
        await TaskExtensions.WaitForAutoCompleteAsync(player.StartAutoAsync());

        Assert.Equal(DictationState.Stopped, player.State);
        Assert.Contains(DictationState.AutoRunning, stateChanges);
    }

    [Fact]
    public async Task PauseAuto_ResumeAuto_StateAutoRunningToAutoPausedToAutoRunning()
    {
        var wordList = new WordListDocument("a\nb\nc\nd\ne\nf\ng\nh\ni\nj");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer())
        {
            Settings = { IntervalExpression = "0", TimesPerWord = 1 }
        };

        var stateChanges = new List<DictationState>();
        player.StateChanged += (_, state) => stateChanges.Add(state);

        await player.StartAutoAsync();

        while (player.State != DictationState.AutoRunning)
        {
            await Task.Delay(10);
        }

        player.PauseAuto();
        Assert.Equal(DictationState.AutoPaused, player.State);

        player.ResumeAuto();
        Assert.Equal(DictationState.AutoRunning, player.State);

        await player.StopAsync();
    }

    [Fact]
    public async Task StopAsync_DuringAuto_StateAutoRunningToStopped()
    {
        var wordList = new WordListDocument("a\nb\nc\nd\ne");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer())
        {
            Settings = { IntervalExpression = "0", TimesPerWord = 1 }
        };

        await player.StartAutoAsync();
        await Task.Delay(50);
        await player.StopAsync();

        Assert.Equal(DictationState.Stopped, player.State);
    }

    [Fact]
    public async Task SpeakAtAsync_EmptyWordList_ReturnsImmediately()
    {
        var wordList = new WordListDocument("");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer());

        var before = DateTime.UtcNow;
        await player.SpeakAtAsync(0);
        var elapsed = DateTime.UtcNow - before;

        Assert.True(elapsed.TotalMilliseconds < 100);
        Assert.Equal(DictationState.Stopped, player.State);
    }

    [Fact]
    public async Task StartAutoAsync_EmptyWordList_ReturnsImmediately()
    {
        var wordList = new WordListDocument("");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer());

        var before = DateTime.UtcNow;
        await player.StartAutoAsync();
        var elapsed = DateTime.UtcNow - before;

        Assert.True(elapsed.TotalMilliseconds < 100);
        Assert.Equal(DictationState.Stopped, player.State);
    }

    [Fact]
    public async Task ProgressChanged_FiresWithCorrectIndices()
    {
        var wordList = new WordListDocument("a\nb\nc");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer())
        {
            Settings = { IntervalExpression = "0", TimesPerWord = 1 }
        };

        var progressEvents = new List<DictationProgress>();
        player.ProgressChanged += (_, progress) => progressEvents.Add(progress);

        await player.StartAutoAsync();
        await TaskExtensions.WaitForAutoCompleteAsync(player.StartAutoAsync());

        Assert.Contains(progressEvents, p => p.CurrentWordIndex == 0 && p.CurrentRepeat == 1);
        Assert.Contains(progressEvents, p => p.CurrentWordIndex == 1 && p.CurrentRepeat == 1);
        Assert.Contains(progressEvents, p => p.CurrentWordIndex == 2 && p.CurrentRepeat == 1);
    }

    [Fact]
    public async Task SpeakAtAsync_ClampsOutOfRangeIndices()
    {
        var wordList = new WordListDocument("apple\nbanana");
        var ttsEngine = new FakePcmTtsEngine();
        var player = new DictationPlayer(ttsEngine, wordList, new FakeAudioPlayer());

        await player.SpeakAtAsync(100);
        Assert.Equal(1, player.Progress.CurrentWordIndex);

        await player.SpeakAtAsync(-5);
        Assert.Equal(0, player.Progress.CurrentWordIndex);
    }
}

public class WavReaderTests
{
    [Fact]
    public void TryReadPcmAudio_ValidWav_ReturnsTrueAndAudio()
    {
        var wavBytes = CreateValidWavBytes(16000, 1, 16, [0x00, 0x00, 0x01, 0x00]);

        var result = WavReader.TryReadPcmAudio(wavBytes, out var audio, out var error);

        Assert.True(result, error);
        Assert.NotNull(audio);
        Assert.Equal(16000, audio!.Format.SampleRate);
        Assert.Equal(1, audio.Format.Channels);
        Assert.Equal(PcmSampleFormat.S16LE, audio.Format.SampleFormat);
    }

    [Fact]
    public void TryReadPcmAudio_TruncatedWav_ReturnsFalse()
    {
        var wavBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };

        var result = WavReader.TryReadPcmAudio(wavBytes, out var audio, out var error);

        Assert.False(result);
        Assert.Null(audio);
        Assert.NotNull(error);
    }

    [Fact]
    public void TryReadPcmAudio_RandomBytes_ReturnsFalse()
    {
        var randomBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

        var result = WavReader.TryReadPcmAudio(randomBytes, out var audio, out var error);

        Assert.False(result);
        Assert.Null(audio);
        Assert.NotNull(error);
    }

    [Fact]
    public void TryReadPcmAudio_InvalidFormatTag_ReturnsFalse()
    {
        var wavBytes = CreateWavBytesWithFormat(16000, 1, 16, formatTag: 0x0080);

        var result = WavReader.TryReadPcmAudio(wavBytes, out var audio, out var error);

        Assert.False(result);
        Assert.Null(audio);
        Assert.Contains("Unsupported WAV format tag", error);
    }

    [Fact]
    public void TryReadPcmAudio_UnsupportedBitsPerSample_ReturnsFalse()
    {
        var wavBytes = CreateValidWavBytes(16000, 1, 8, [0x00]);

        var result = WavReader.TryReadPcmAudio(wavBytes, out var audio, out var error);

        Assert.False(result);
        Assert.Null(audio);
        Assert.Contains("Unsupported WAV bits-per-sample", error);
    }

    private static byte[] CreateValidWavBytes(int sampleRate, int channels, int bitsPerSample, byte[] data)
    {
        var byteRate = sampleRate * channels * bitsPerSample / 8;
        var blockAlign = channels * bitsPerSample / 8;
        var dataSize = data.Length;

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        bw.Write("RIFF"u8.ToArray());
        bw.Write(36 + dataSize);
        bw.Write("WAVE"u8.ToArray());

        bw.Write("fmt "u8.ToArray());
        bw.Write(16);
        bw.Write((ushort)1);
        bw.Write((ushort)channels);
        bw.Write(sampleRate);
        bw.Write(byteRate);
        bw.Write((ushort)blockAlign);
        bw.Write((ushort)bitsPerSample);

        bw.Write("data"u8.ToArray());
        bw.Write(dataSize);
        bw.Write(data);

        return ms.ToArray();
    }

    private static byte[] CreateWavBytesWithFormat(int sampleRate, int channels, int bitsPerSample, ushort formatTag)
    {
        var byteRate = sampleRate * channels * bitsPerSample / 8;
        var blockAlign = channels * bitsPerSample / 8;
        var data = new byte[] { 0x00, 0x00 };

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        bw.Write("RIFF"u8.ToArray());
        bw.Write(36 + data.Length);
        bw.Write("WAVE"u8.ToArray());

        bw.Write("fmt "u8.ToArray());
        bw.Write(16);
        bw.Write(formatTag);
        bw.Write((ushort)channels);
        bw.Write(sampleRate);
        bw.Write(byteRate);
        bw.Write((ushort)blockAlign);
        bw.Write((ushort)bitsPerSample);

        bw.Write("data"u8.ToArray());
        bw.Write(data.Length);
        bw.Write(data);

        return ms.ToArray();
    }
}
