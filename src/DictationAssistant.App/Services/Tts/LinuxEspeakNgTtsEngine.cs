using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.Services.Tts;

public sealed class LinuxEspeakNgTtsEngine : ITtsEngine, IConfigurableTtsEngine, IPcmTtsEngine
{
    public string Name => "Linux espeak-ng";

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
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-espeak-{Guid.NewGuid():N}.wav");
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
                args.Add("-s");
                args.Add((160 + Math.Clamp(rate, -10, 10) * 15).ToString());
            }

            args.Add("-w");
            args.Add(tempFile);
            args.Add(text);

            await ProcessRunner.RunAsync("espeak-ng", args, null, cancellationToken).ConfigureAwait(false);
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
