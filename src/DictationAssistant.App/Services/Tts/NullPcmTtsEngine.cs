using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class NullPcmTtsEngine : IPcmTtsEngine
{
    public string Name => "Null PCM TTS (No-op)";

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
        return Task.FromResult<PcmAudio?>(null);
    }
}
