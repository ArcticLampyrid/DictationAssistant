using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class MacSayTtsEngine : ITtsEngine, IConfigurableTtsEngine, IPcmTtsEngine
{
    public string Name => "macOS say";

    public Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        _ = text;
        _ = cancellationToken;
        return Task.CompletedTask;
    }

    public Task SpeakAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        _ = text;
        _ = options;
        _ = cancellationToken;
        return Task.CompletedTask;
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

                voices.Add(new TtsVoiceInfo
                {
                    Name = tokens[0],
                    LocaleOrLanguage = tokens.Length > 1 ? tokens[1] : null
                });
            }

            return voices;
        }
        catch
        {
            return [];
        }
    }

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        var wavBytes = await SynthesizeWavAsync(text, options, ct).ConfigureAwait(false);
        if (wavBytes is null)
        {
            return null;
        }

        return WavReader.TryReadPcmAudio(wavBytes, out var pcmAudio, out _)
            ? pcmAudio
            : null;
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        return SynthesizeWavAsync(text, new TtsSpeakOptions(), cancellationToken);
    }

    public Task<byte[]?> SynthesizeAudioAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        return SynthesizeWavAsync(text, options, cancellationToken);
    }

    private static async Task<byte[]?> SynthesizeWavAsync(string text, TtsSpeakOptions options, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-say-{Guid.NewGuid():N}.wav");
        try
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

            args.Add("-o");
            args.Add(tempFile);
            args.Add("--file-format=WAVE");
            args.Add("--data-format=LEI16@44100");
            args.Add(text);

            try
            {
                await ProcessRunner.RunAsync("say", args, null, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                var fallbackArgs = new List<string>();
                if (!string.IsNullOrWhiteSpace(options.VoiceName))
                {
                    fallbackArgs.Add("-v");
                    fallbackArgs.Add(options.VoiceName);
                }

                if (options.Rate is int fallbackRate)
                {
                    fallbackArgs.Add("-r");
                    fallbackArgs.Add((120 + Math.Clamp(fallbackRate, -10, 10) * 20).ToString());
                }

                fallbackArgs.Add("-o");
                fallbackArgs.Add(tempFile);
                fallbackArgs.Add(text);
                await ProcessRunner.RunAsync("say", fallbackArgs, null, cancellationToken).ConfigureAwait(false);
            }

            return await File.ReadAllBytesAsync(tempFile, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
