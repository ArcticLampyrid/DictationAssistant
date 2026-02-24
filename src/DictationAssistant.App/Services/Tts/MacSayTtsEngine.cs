using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public sealed class MacSayTtsEngine : ITtsEngine
{
    public string Name => "macOS say";

    public async Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        await ProcessRunner.RunAsync("say", [text], null, cancellationToken).ConfigureAwait(false);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.FromResult<byte[]?>(null);
    }
}
