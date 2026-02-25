using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class LinuxPico2WaveTtsEngine : IPcmTtsEngine
{
    public string Name => "Linux pico2wave";

    public Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken cancellationToken)
    {
        _ = cancellationToken;
        return Task.FromResult<IReadOnlyList<TtsVoiceInfo>>([]);
    }

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        var wavBytes = await SynthesizeWavAsync(text, options, ct).ConfigureAwait(false);
        if (wavBytes is null)
        {
            return null;
        }

        return WavReader.TryReadPcmAudio(wavBytes, out var pcmAudio, out _)
            ? pcmAudio
            : null;
    }

    private static async Task<byte[]?> SynthesizeWavAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-pico-{Guid.NewGuid():N}.wav");
        try
        {
            var voice = string.IsNullOrWhiteSpace(options.VoiceName) ? "zh-CN" : options.VoiceName;
            await ProcessRunner.RunAsync("pico2wave", ["-l", voice, "-w", tempFile, text], null, cancellationToken).ConfigureAwait(false);
            return await File.ReadAllBytesAsync(tempFile, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
