using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public sealed class WindowsSystemSpeechTtsEngine : ITtsEngine
{
    private const string Script = "Add-Type -AssemblyName System.Speech; $s = New-Object System.Speech.Synthesis.SpeechSynthesizer; $text = [Console]::In.ReadToEnd(); $s.Speak($text);";

    public string Name => "Windows System.Speech";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        return ProcessRunner.RunAsync("powershell", ["-NoProfile", "-Command", Script], text, cancellationToken);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.FromResult<byte[]?>(null);
    }
}
