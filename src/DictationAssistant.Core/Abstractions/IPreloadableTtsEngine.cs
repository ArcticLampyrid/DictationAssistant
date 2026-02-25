using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IPreloadableTtsEngine
{
    Task PreloadAsync(string text, TtsSpeakOptions options, CancellationToken ct);

    Task<PcmAudio?> TryConsumePreloadedAsync(string text, TtsSpeakOptions options, CancellationToken ct);
}
