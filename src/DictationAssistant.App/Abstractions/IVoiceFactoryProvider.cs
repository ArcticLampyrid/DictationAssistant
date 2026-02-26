using DictationAssistant.Abstractions;

namespace DictationAssistant.Abstractions;

public interface IVoiceFactoryProvider
{
    Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct);
}
