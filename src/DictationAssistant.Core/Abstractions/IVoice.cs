using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IVoice
{
    string Name { get; }

    Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
