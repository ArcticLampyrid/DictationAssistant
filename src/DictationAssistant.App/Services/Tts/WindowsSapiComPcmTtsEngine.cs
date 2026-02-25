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

    private static readonly string[] VoiceCategoryIds =
    [
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices"
    ];

    [SupportedOSPlatform("windows")]
    private static IReadOnlyList<TtsVoiceInfo> ListVoicesWindows()
    {
        var results = new List<TtsVoiceInfo>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var categoryId in VoiceCategoryIds)
        {
            object? categoryObj = null;
            try
            {
                categoryObj = CreateComObject("SAPI.SpObjectTokenCategory");
                if (categoryObj is null)
                {
                    continue;
                }

                dynamic category = categoryObj;
                category.SetId(categoryId, false);
                dynamic tokens = category.EnumerateTokens();
                var count = (int)tokens.Count;

                for (var i = 0; i < count; i++)
                {
                    dynamic token = tokens.Item(i);
                    var name = SafeToString(token.GetDescription());
                    if (!string.IsNullOrWhiteSpace(name) && seen.Add(name))
                    {
                        var locale = ParseSapiLanguageHex(SafeToString(token.GetAttribute("Language")));
                        results.Add(new TtsVoiceInfo
                        {
                            Name = name,
                            LocaleOrLanguage = locale
                        });
                    }
                    ReleaseCom(token);
                }

                ReleaseCom(tokens);
            }
            catch
            {
                // Category may not exist on this system — skip silently
            }
            finally
            {
                ReleaseCom(categoryObj);
            }
        }

        return results;
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
                var selectedToken = FindVoiceToken(options.VoiceName);
                if (selectedToken is not null)
                {
                    try
                    {
                        voice.Voice = selectedToken;
                    }
                    finally
                    {
                        ReleaseCom(selectedToken);
                    }
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
    private static object? FindVoiceToken(string voiceName)
    {
        foreach (var categoryId in VoiceCategoryIds)
        {
            object? categoryObj = null;
            try
            {
                categoryObj = CreateComObject("SAPI.SpObjectTokenCategory");
                if (categoryObj is null)
                {
                    continue;
                }

                dynamic category = categoryObj;
                category.SetId(categoryId, false);
                dynamic tokens = category.EnumerateTokens();
                var count = (int)tokens.Count;

                for (var i = 0; i < count; i++)
                {
                    dynamic token = tokens.Item(i);
                    var name = SafeToString(token.GetDescription());
                    if (string.Equals(name, voiceName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReleaseCom(tokens);
                        return token; // caller must ReleaseCom
                    }
                    ReleaseCom(token);
                }

                ReleaseCom(tokens);
            }
            catch
            {
            }
            finally
            {
                ReleaseCom(categoryObj);
            }
        }

        return null;
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
