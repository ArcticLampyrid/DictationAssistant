using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.Core.Abstractions;

public interface IVoiceFactoryProvider
{
    Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct);
}
