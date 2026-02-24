namespace DictationAssistant.Core.Abstractions;

public interface ITtsEngine
{
    string Name { get; }

    Task SpeakAsync(string text, CancellationToken cancellationToken);

    Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken);
}
