using System.Diagnostics;
using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;
using EdgeTTS.DotNet;
using EdgeTTS.DotNet.Models;

namespace DictationAssistant.App.Voice;

public sealed class EdgeTtsVoice : CachedVoice
{
    private readonly string _voiceName;

    public EdgeTtsVoice(string voiceName)
    {
        _voiceName = voiceName;
    }

    protected override async Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            var rate = MapRate(options.Rate);
            var communicate = new Communicate(text, voice: _voiceName, rate: rate);

            var mp3Bytes = new List<byte>();

            await foreach (var chunk in communicate.StreamAsync(ct))
            {
                if (chunk is AudioChunk audio)
                {
                    mp3Bytes.AddRange(audio.Data);
                }
            }

            if (mp3Bytes.Count == 0)
            {
                Trace.WriteLine("[EdgeTTS] No audio data received");
                return null;
            }

            return DecodeMp3ToPcm([.. mp3Bytes]);
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



    private static PcmAudio? DecodeMp3ToPcm(byte[] mp3Bytes)
    {
        try
        {
            using var mp3Stream = new MemoryStream(mp3Bytes);
            using var decodeStream = BassAudioDecoder.DecodeStream(mp3Stream);
            if (decodeStream == null)
            {
                return null;
            }

            var format = decodeStream.Format;

            using var pcmStream = new MemoryStream();
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = decodeStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                pcmStream.Write(buffer, 0, bytesRead);
            }

            return new PcmAudio
            {
                Data = pcmStream,
                Format = format
            };
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Audio decode failed: {ex.Message}");
            return null;
        }
    }
}

public sealed class EdgeTtsVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;
    private readonly string _internalName;

    public EdgeTtsVoiceFactory(VoiceInfo info, string internalName)
    {
        _info = info;
        _internalName = internalName;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new EdgeTtsVoice(_internalName);
    }

    public override string ToString() => _info.DisplayName;
}

public sealed class EdgeTtsVoiceFactoryProvider : IVoiceFactoryProvider
{
    public async Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct)
    {
        try
        {
            var voices = await Voices.ListVoicesAsync().ConfigureAwait(false);
            return voices.Select(v => new EdgeTtsVoiceFactory(new VoiceInfo
            {
                Id = $"edge-tts:{v.ShortName}",
                DisplayName = v.FriendlyName ?? v.ShortName,
                LocaleOrLanguage = v.Locale,
            }, v.ShortName)).ToList();
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Failed to list voices: {ex.Message}");
            return [];
        }
    }
}
