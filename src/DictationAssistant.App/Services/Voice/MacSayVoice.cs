using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;

namespace DictationAssistant.App.Services.Voice;

public sealed class MacSayVoice : CachedVoice
{
    private readonly string _voiceName;

    public MacSayVoice(string voiceName)
    {
        _voiceName = voiceName;
    }

    public override string Name => _voiceName;

    protected override async Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
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

    private async Task<byte[]?> SynthesizeWavAsync(string text, VoiceSynthesisOptions options, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-say-{Guid.NewGuid():N}.wav");
        try
        {
            var args = new List<string>();
            if (!string.IsNullOrWhiteSpace(_voiceName))
            {
                args.Add("-v");
                args.Add(_voiceName);
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
                if (!string.IsNullOrWhiteSpace(_voiceName))
                {
                    fallbackArgs.Add("-v");
                    fallbackArgs.Add(_voiceName);
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

public sealed class MacSayVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;

    public MacSayVoiceFactory(VoiceInfo info)
    {
        _info = info;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new MacSayVoice(_info.DisplayName);
    }
}

public sealed class MacSayVoiceFactoryProvider : IVoiceFactoryProvider
{
    public async Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var output = await ProcessRunner.RunCaptureAsync("say", ["-v", "?"], null, cancellationToken).ConfigureAwait(false);
            var voices = new List<IVoiceFactory>();
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

                var info = new VoiceInfo
                {
                    Id = $"say:{tokens[0]}",
                    DisplayName = tokens[0],
                    LocaleOrLanguage = tokens.Length > 1 ? tokens[1] : null,
                    ProviderName = "macOS say"
                };
                voices.Add(new MacSayVoiceFactory(info));
            }

            return voices;
        }
        catch
        {
            return [];
        }
    }
}
