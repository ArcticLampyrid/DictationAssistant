using DictationAssistant.Audio;
using DictationAssistant.Models;

namespace DictationAssistant.Abstractions;

public interface IPreloadableVoice : IVoice
{
    Task PreloadAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
