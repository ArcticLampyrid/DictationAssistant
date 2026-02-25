using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class LinuxEspeakNgTtsEngine : ITtsEngine, IConfigurableTtsEngine
{
    public string Name => "Linux espeak-ng";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        return SpeakAsync(text, new TtsSpeakOptions(), cancellationToken);
    }

    public async Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var output = await ProcessRunner.RunCaptureAsync("espeak-ng", ["--voices"], null, cancellationToken).ConfigureAwait(false);
            var voices = new List<TtsVoiceInfo>();
            using var reader = new StringReader(output);
            string? line;
            while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is not null)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("Pty") || trimmed.StartsWith("-") || !char.IsDigit(trimmed[0]))
                {
                    continue;
                }

                var columns = trimmed.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
                if (columns.Length < 4)
                {
                    continue;
                }

                voices.Add(new TtsVoiceInfo
                {
                    Name = columns[3],
                    LocaleOrLanguage = columns[1]
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
            args.Add("-s");
            args.Add((160 + Math.Clamp(rate, -10, 10) * 15).ToString());
        }

        if (options.Volume is int volume)
        {
            args.Add("-a");
            args.Add((Math.Clamp(volume, 0, 100) * 2).ToString());
        }

        args.Add(text);
        return ProcessRunner.RunAsync("espeak-ng", args, null, cancellationToken);
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
