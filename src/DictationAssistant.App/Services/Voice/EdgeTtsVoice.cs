using System.Diagnostics;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;
using EdgeTTS;
using MP3Sharp;

namespace DictationAssistant.App.Services.Voice;

public sealed class EdgeTtsVoice : CachedVoice
{
    private readonly string _voiceName;

    public EdgeTtsVoice(string voiceName)
    {
        _voiceName = voiceName;
    }

    public override string Name => _voiceName;

    protected override async Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            var rate = MapRate(options.Rate);
            var volume = MapVolume(options.Volume);

            var communicate = new Communicate(text, _voiceName, rate, volume);

            using var ms = new MemoryStream();

            await communicate.Stream(result =>
            {
                if (result.Type == "Audio")
                {
                    result.Data?.CopyTo(ms);
                }
            }, ct).ConfigureAwait(false);

            var mp3Bytes = ms.ToArray();
            if (mp3Bytes.Length == 0)
            {
                Trace.WriteLine("[EdgeTTS] No audio data received");
                return null;
            }

            return DecodeMp3ToPcm(mp3Bytes);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Synthesis failed: {ex.Message}");
            return null;
        }
    }

    private static string MapRate(int? rate)
    {
        if (rate is null)
        {
            return "+0%";
        }

        var edgeRate = rate.Value * 10;
        return edgeRate >= 0 ? $"+{edgeRate}%" : $"{edgeRate}%";
    }

    private static string MapVolume(int? volume)
    {
        if (volume is null)
        {
            return "+0%";
        }

        var edgeVolume = Math.Clamp(volume.Value, -10, 10) * 10;
        return edgeVolume >= 0 ? $"+{edgeVolume}%" : $"{edgeVolume}%";
    }

    private static PcmAudio? DecodeMp3ToPcm(byte[] mp3Bytes)
    {
        try
        {
            using var mp3Stream = new MP3Sharp.MP3Stream(new MemoryStream(mp3Bytes));
            var sampleRate = mp3Stream.Frequency;
            var channels = mp3Stream.ChannelCount;

            using var pcmBuffer = new MemoryStream();
            var buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = mp3Stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                pcmBuffer.Write(buffer, 0, bytesRead);
            }

            if (pcmBuffer.Length == 0)
            {
                Trace.WriteLine("[EdgeTTS] No samples decoded from MP3");
                return null;
            }

            return new PcmAudio
            {
                Data = pcmBuffer.ToArray(),
                Format = new PcmFormatInfo(sampleRate, channels, PcmSampleFormat.S16LE)
            };
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] MP3 decode failed: {ex.Message}");
            return null;
        }
    }
}

public sealed class EdgeTtsVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;

    public EdgeTtsVoiceFactory(VoiceInfo info)
    {
        _info = info;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new EdgeTtsVoice(_info.DisplayName);
    }
}

public sealed class EdgeTtsVoiceFactoryProvider : IVoiceFactoryProvider
{
    public async Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct)
    {
        try
        {
            var voices = await VoicesManager.ListVoices(null, ct).ConfigureAwait(false);
            return voices.Select(v => new EdgeTtsVoiceFactory(new VoiceInfo
            {
                Id = $"edge:{v.Name}",
                DisplayName = v.Name,
                LocaleOrLanguage = v.Locale,
                ProviderName = "Edge TTS"
            })).ToList();
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Failed to list voices: {ex.Message}");
            return [];
        }
    }
}
