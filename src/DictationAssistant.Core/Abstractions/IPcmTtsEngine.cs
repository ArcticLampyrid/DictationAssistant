using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IPcmTtsEngine
{
    string Name { get; }

    Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken ct);

    Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct);
}
