using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;

namespace DictationAssistant.App.Abstractions;

public interface IVoice
{
    string Name { get; }

    Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
