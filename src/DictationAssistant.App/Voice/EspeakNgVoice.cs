using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;

namespace DictationAssistant.App.Voice;

public sealed class EspeakNgVoice : IVoice
{
    private readonly string _voiceName;

    public EspeakNgVoice(string voiceName)
    {
        _voiceName = voiceName;
    }

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        var wavBytes = await SynthesizeWavAsync(text, options, ct).ConfigureAwait(false);
        if (wavBytes is null)
        {
            return null;
        }
        var pcmStream = BassAudioDecoder.DecodeStream(new MemoryStream(wavBytes));
        if (pcmStream is null)
        {
            return null;
        }
        return new PcmAudio()
        {
            Data = pcmStream,
            Format = pcmStream.Format
        };
    }

    private async Task<byte[]?> SynthesizeWavAsync(string text, VoiceSynthesisOptions options, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-espeak-{Guid.NewGuid():N}.wav");
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

public sealed class EspeakNgVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;
    private readonly string _voiceName;

    public EspeakNgVoiceFactory(VoiceInfo info, string voiceName)
    {
        _info = info;
        _voiceName = voiceName;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new EspeakNgVoice(_voiceName);
    }

    public override string ToString() => _info.DisplayName;
}

public sealed class EspeakNgVoiceFactoryProvider : IVoiceFactoryProvider
{
    public async Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsLinux())
        {
            return [];
        }
        try
        {
            var output = await ProcessRunner.RunCaptureAsync("espeak-ng", ["--voices"], null, cancellationToken).ConfigureAwait(false);
            var voices = new List<IVoiceFactory>();
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
                var voiceName = columns[3].Replace('_', ' ');
                var info = new VoiceInfo
                {
                    Id = $"espeak:{columns[3]}",
                    DisplayName = $"{voiceName} (eSpeak NG)",
                    LocaleOrLanguage = columns[1],
                };
                voices.Add(new EspeakNgVoiceFactory(info, voiceName));
            }

            return voices;
        }
        catch
        {
            return [];
        }
    }
}
