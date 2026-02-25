using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IPreloadableVoice : IVoice
{
    Task PreloadAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
