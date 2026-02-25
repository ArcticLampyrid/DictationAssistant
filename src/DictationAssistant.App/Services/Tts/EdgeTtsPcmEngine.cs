using System.Diagnostics;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using EdgeTTS;
using MP3Sharp;

namespace DictationAssistant.App.Services.Tts;

public sealed class EdgeTtsPcmEngine : IPcmTtsEngine, IPreloadableTtsEngine
{
    private (string Text, string VoiceName, int Rate)? _preloadedKey;
    private PcmAudio? _preloadedValue;
    private readonly object _lock = new();

    public string Name => "Edge TTS";

    public async Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken ct)
    {
        try
        {
            var voices = await VoicesManager.ListVoices(null, ct).ConfigureAwait(false);
            return voices.Select(v => new TtsVoiceInfo
            {
                Name = v.Name,
                LocaleOrLanguage = v.Locale
            }).ToList();
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Failed to list voices: {ex.Message}");
            return [];
        }
    }

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            var voiceName = options.VoiceName ?? "zh-CN-XiaoyiNeural";
            var rate = MapRate(options.Rate);
            var volume = MapVolume(options.Volume);

            var communicate = new Communicate(text, voiceName, rate, volume);

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

    public async Task PreloadAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        try
        {
            var pcmAudio = await SynthesizePcmAsync(text, options, ct).ConfigureAwait(false);
            if (pcmAudio is null)
            {
                return;
            }

            lock (_lock)
            {
                _preloadedKey = (text, options.VoiceName ?? "zh-CN-XiaoyiNeural", options.Rate ?? 0);
                _preloadedValue = pcmAudio;
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[EdgeTTS] Preload failed: {ex.Message}");
        }
    }

    public Task<PcmAudio?> TryConsumePreloadedAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        var voiceName = options.VoiceName ?? "zh-CN-XiaoyiNeural";
        var rate = options.Rate ?? 0;

        lock (_lock)
        {
            if (_preloadedKey is not null &&
                _preloadedKey.Value.Text == text &&
                _preloadedKey.Value.VoiceName == voiceName &&
                _preloadedKey.Value.Rate == rate &&
                _preloadedValue is not null)
            {
                var result = _preloadedValue;
                _preloadedKey = null;
                _preloadedValue = null;
                return Task.FromResult<PcmAudio?>(result);
            }
        }

        return Task.FromResult<PcmAudio?>(null);
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
