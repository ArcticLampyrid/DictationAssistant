using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public sealed class LinuxEspeakNgTtsEngine : ITtsEngine
{
    public string Name => "Linux espeak-ng";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        return ProcessRunner.RunAsync("espeak-ng", [text], null, cancellationToken);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.FromResult<byte[]?>(null);
    }
}
