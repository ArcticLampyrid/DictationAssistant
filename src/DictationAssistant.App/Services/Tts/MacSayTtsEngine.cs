using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class MacSayTtsEngine : ITtsEngine, IConfigurableTtsEngine
{
    public string Name => "macOS say";

    public async Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        await SpeakAsync(text, new TtsSpeakOptions(), cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var output = await ProcessRunner.RunCaptureAsync("say", ["-v", "?"], null, cancellationToken).ConfigureAwait(false);
            var voices = new List<TtsVoiceInfo>();
            using var reader = new StringReader(output);
            string? line;
            while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is not null)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                {
                    continue;
                }

                var locale = tokens.Length > 1 ? tokens[1] : null;
                voices.Add(new TtsVoiceInfo
                {
                    Name = tokens[0],
                    LocaleOrLanguage = locale
                });
            }

            return voices;
        }
        catch
        {
            return [];
        }
    }

    public Task SpeakAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        var args = new List<string>();
        if (!string.IsNullOrWhiteSpace(options.VoiceName))
        {
            args.Add("-v");
            args.Add(options.VoiceName);
        }

        if (options.Rate is int rate)
        {
            args.Add("-r");
            args.Add((120 + Math.Clamp(rate, -10, 10) * 20).ToString());
        }

        args.Add(text);
        return ProcessRunner.RunAsync("say", args, null, cancellationToken);
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
