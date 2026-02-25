using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class WindowsSystemSpeechTtsEngine : ITtsEngine, IConfigurableTtsEngine
{
    private const string ListVoicesScript = "Add-Type -AssemblyName System.Speech; $s = New-Object System.Speech.Synthesis.SpeechSynthesizer; $s.GetInstalledVoices() | ForEach-Object { $_.VoiceInfo.Name };";
    private const string SpeakScript = "Add-Type -AssemblyName System.Speech; $payload = [Console]::In.ReadToEnd() | ConvertFrom-Json; $s = New-Object System.Speech.Synthesis.SpeechSynthesizer; if ($payload.Volume -ne $null) { $s.Volume = [int]$payload.Volume }; if ($payload.Rate -ne $null) { $s.Rate = [int]$payload.Rate }; if ($payload.VoiceName) { try { $s.SelectVoice([string]$payload.VoiceName) } catch {} }; $s.Speak([string]$payload.Text);";

    public string Name => "Windows System.Speech";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        return SpeakAsync(text, new TtsSpeakOptions(), cancellationToken);
    }

    public async Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var output = await ProcessRunner.RunCaptureAsync("powershell", ["-NoProfile", "-Command", ListVoicesScript], null, cancellationToken).ConfigureAwait(false);
            var voices = output
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(name => new TtsVoiceInfo { Name = name })
                .ToArray();
            return voices;
        }
        catch
        {
            return [];
        }
    }

    public Task SpeakAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        var payload = new
        {
            Text = text,
            VoiceName = string.IsNullOrWhiteSpace(options.VoiceName) ? null : options.VoiceName,
            Volume = options.Volume is int volume ? (int?)Math.Clamp(volume, 0, 100) : null,
            Rate = options.Rate is int rate ? (int?)Math.Clamp(rate, -10, 10) : null
        };
        var json = System.Text.Json.JsonSerializer.Serialize(payload);
        return ProcessRunner.RunAsync("powershell", ["-NoProfile", "-Command", SpeakScript], json, cancellationToken);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.FromResult<byte[]?>(null);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        _ = options;
        return SynthesizeAudioAsync(text, cancellationToken);
    }
}
