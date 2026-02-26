using DictationAssistant.Audio;
using DictationAssistant.Models;

namespace DictationAssistant.Abstractions;

public interface IVoice
{
    string Name { get; }

    Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
