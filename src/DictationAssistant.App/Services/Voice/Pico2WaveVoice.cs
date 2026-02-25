using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;

namespace DictationAssistant.App.Services.Voice;

public sealed class Pico2WaveVoice : CachedVoice
{
    private readonly string _language;

    public Pico2WaveVoice(string language)
    {
        _language = language;
    }

    public override string Name => _language;

    protected override async Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        var wavBytes = await SynthesizeWavAsync(text, ct).ConfigureAwait(false);
        if (wavBytes is null)
        {
            return null;
        }

        return WavReader.TryReadPcmAudio(wavBytes, out var pcmAudio, out _)
            ? pcmAudio
            : null;
    }

    private async Task<byte[]?> SynthesizeWavAsync(string text, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-pico-{Guid.NewGuid():N}.wav");
        try
        {
            await ProcessRunner.RunAsync("pico2wave", ["-l", _language, "-w", tempFile, text], null, cancellationToken).ConfigureAwait(false);
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

public sealed class Pico2WaveVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;

    public Pico2WaveVoiceFactory(VoiceInfo info)
    {
        _info = info;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new Pico2WaveVoice(_info.DisplayName);
    }
}

public sealed class Pico2WaveVoiceFactoryProvider : IVoiceFactoryProvider
{
    private static readonly IReadOnlyList<VoiceInfo> SupportedLanguages =
    [
        new VoiceInfo { Id = "pico:en-US", DisplayName = "en-US", LocaleOrLanguage = "en-US", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:en-GB", DisplayName = "en-GB", LocaleOrLanguage = "en-GB", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:de-DE", DisplayName = "de-DE", LocaleOrLanguage = "de-DE", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:es-ES", DisplayName = "es-ES", LocaleOrLanguage = "es-ES", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:fr-FR", DisplayName = "fr-FR", LocaleOrLanguage = "fr-FR", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:it-IT", DisplayName = "it-IT", LocaleOrLanguage = "it-IT", ProviderName = "pico2wave" },
        new VoiceInfo { Id = "pico:zh-CN", DisplayName = "zh-CN", LocaleOrLanguage = "zh-CN", ProviderName = "pico2wave" }
    ];

    public Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct)
    {
        _ = ct;
        return Task.FromResult<IReadOnlyList<IVoiceFactory>>(
            SupportedLanguages.Select(l => new Pico2WaveVoiceFactory(l)).ToList()
        );
    }
}
