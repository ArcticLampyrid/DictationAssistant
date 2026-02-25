using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IConfigurableTtsEngine
{
    Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken cancellationToken);

    Task SpeakAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken);

    Task<byte[]?> SynthesizeAudioAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken);
}
