using DictationAssistant.Abstractions;
using DictationAssistant.Audio;
using DictationAssistant.Models;

namespace DictationAssistant.Services;

public sealed class NullVoice : IVoice
{
    public static NullVoice Instance { get; } = new();

    public string Name => "Null Voice";

    public Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        _ = text;
        _ = options;
        _ = ct;
        return Task.FromResult<PcmAudio?>(null);
    }
}
