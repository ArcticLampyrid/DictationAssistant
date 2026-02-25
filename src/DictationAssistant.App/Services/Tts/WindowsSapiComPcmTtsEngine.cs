using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DictationAssistant.App.Services.Tts;

public sealed class WindowsSapiComPcmTtsEngine : IPcmTtsEngine
{
    private const int Saft44Khz16BitStereo = 39;

    public string Name => "Windows SAPI COM";

    public Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken ct)
    {
        if (!OperatingSystem.IsWindows())
        {
            return Task.FromResult<IReadOnlyList<TtsVoiceInfo>>([]);
        }

        ct.ThrowIfCancellationRequested();
        return ListVoicesOnWindowsAsync(ct);
    }

    public Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        if (!OperatingSystem.IsWindows())
        {
            return Task.FromResult<PcmAudio?>(null);
        }

        ct.ThrowIfCancellationRequested();
        return SynthesizeOnWindowsAsync(text, options, ct);
    }

    [SupportedOSPlatform("windows")]
    private static Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesOnWindowsAsync(CancellationToken ct)
    {
        return Task.Run<IReadOnlyList<TtsVoiceInfo>>(ListVoicesWindows, ct);
    }

    [SupportedOSPlatform("windows")]
    private static Task<PcmAudio?> SynthesizeOnWindowsAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        return Task.Run(() => SynthesizeWindows(text, options), ct);
    }

    [SupportedOSPlatform("windows")]
    private static IReadOnlyList<TtsVoiceInfo> ListVoicesWindows()
    {
        object? voiceObj = null;
        object? voicesObj = null;

        try
        {
            voiceObj = CreateComObject("SAPI.SpVoice");
            if (voiceObj is null)
            {
                return [];
            }

            dynamic voice = voiceObj;
            voicesObj = voice.GetVoices();
            dynamic voices = voicesObj;

            var count = (int)voices.Count;
            var results = new List<TtsVoiceInfo>(count);
            for (var i = 0; i < count; i++)
            {
                dynamic token = voices.Item(i);
                var name = SafeToString(token.GetDescription());
                var locale = ParseSapiLanguageHex(SafeToString(token.GetAttribute("Language")));
                if (!string.IsNullOrWhiteSpace(name))
                {
                    results.Add(new TtsVoiceInfo
                    {
                        Name = name,
                        LocaleOrLanguage = locale
                    });
                }
                ReleaseCom(token);
            }

            return results;
        }
        catch
        {
            return [];
        }
        finally
        {
            ReleaseCom(voicesObj);
            ReleaseCom(voiceObj);
        }
    }

    [SupportedOSPlatform("windows")]
    private static PcmAudio? SynthesizeWindows(string text, TtsSpeakOptions options)
    {
        object? voiceObj = null;
        object? streamObj = null;
        object? audioFormatObj = null;
        object? voicesObj = null;

        try
        {
            voiceObj = CreateComObject("SAPI.SpVoice");
            streamObj = CreateComObject("SAPI.SpMemoryStream");
            audioFormatObj = CreateComObject("SAPI.SpAudioFormat");
            if (voiceObj is null || streamObj is null || audioFormatObj is null)
            {
                return null;
            }

            dynamic voice = voiceObj;
            dynamic stream = streamObj;
            dynamic audioFormat = audioFormatObj;

            audioFormat.Type = Saft44Khz16BitStereo;
            stream.Format = audioFormat;
            voice.AllowAudioOutputFormatChangesOnNextSet = false;
            voice.AudioOutputStream = stream;
            voice.Rate = options.Rate is int rate ? Math.Clamp(rate, -10, 10) : 0;

            if (!string.IsNullOrWhiteSpace(options.VoiceName))
            {
                voicesObj = voice.GetVoices();
                dynamic voices = voicesObj;
                var count = (int)voices.Count;
                for (var i = 0; i < count; i++)
                {
                    dynamic token = voices.Item(i);
                    var name = SafeToString(token.GetDescription());
                    if (string.Equals(name, options.VoiceName, StringComparison.OrdinalIgnoreCase))
                    {
                        voice.Voice = token;
                        ReleaseCom(token);
                        break;
                    }

                    ReleaseCom(token);
                }
            }

            voice.Speak(text, 0);
            stream.Seek(0, 0);
            var raw = (byte[]?)stream.GetData();
            if (raw is null || raw.Length == 0)
            {
                return null;
            }

            return new PcmAudio
            {
                Data = raw,
                Format = new PcmFormatInfo(44100, 2, PcmSampleFormat.S16LE)
            };
        }
        catch
        {
            return null;
        }
        finally
        {
            ReleaseCom(voicesObj);
            ReleaseCom(audioFormatObj);
            ReleaseCom(streamObj);
            ReleaseCom(voiceObj);
        }
    }

    [SupportedOSPlatform("windows")]
    private static object? CreateComObject(string progId)
    {
        var comType = Type.GetTypeFromProgID(progId, throwOnError: false);
        return comType is null ? null : Activator.CreateInstance(comType);
    }

    private static string SafeToString(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }

    private static string? ParseSapiLanguageHex(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var first = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
        if (first is null)
        {
            return null;
        }

        if (!ushort.TryParse(first, System.Globalization.NumberStyles.HexNumber, null, out var lcid))
        {
            return null;
        }

        try
        {
            return new System.Globalization.CultureInfo(lcid).Name;
        }
        catch
        {
            return null;
        }
    }

    [SupportedOSPlatform("windows")]
    private static void ReleaseCom(object? comObject)
    {
        if (comObject is not null && Marshal.IsComObject(comObject))
        {
            Marshal.FinalReleaseComObject(comObject);
        }
    }
}
