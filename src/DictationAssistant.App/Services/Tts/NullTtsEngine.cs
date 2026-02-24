using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public sealed class NullTtsEngine : ITtsEngine
{
    public string Name => "Null TTS (No-op)";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.CompletedTask;
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.FromResult<byte[]?>(null);
    }
}
