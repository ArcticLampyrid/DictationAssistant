using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DictationAssistant.App.Services.Voice;

public sealed class SapiVoice : IVoice
{
    private const int Saft44Khz16BitStereo = 39;
    private readonly string _voiceName;

    public SapiVoice(string voiceName)
    {
        _voiceName = voiceName;
    }

    public string Name => _voiceName;

    [SupportedOSPlatform("windows")]
    public Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        return Task.Run(() => SynthesizeWindows(text, options, _voiceName), ct);
    }

    [SupportedOSPlatform("windows")]
    private static PcmAudio? SynthesizeWindows(string text, VoiceSynthesisOptions options, string voiceName)
    {
        object? voiceObj = null;
        object? streamObj = null;
        object? audioFormatObj = null;

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

            var selectedToken = FindVoiceToken(voiceName);
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
            ReleaseCom(audioFormatObj);
            ReleaseCom(streamObj);
            ReleaseCom(voiceObj);
        }
    }

    [SupportedOSPlatform("windows")]
    private static object? FindVoiceToken(string voiceName)
    {
        var categoryIds = new[]
        {
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices",
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices",
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices"
        };

        foreach (var categoryId in categoryIds)
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
                        return token;
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

    [SupportedOSPlatform("windows")]
    private static void ReleaseCom(object? comObject)
    {
        if (comObject is not null && Marshal.IsComObject(comObject))
        {
            Marshal.FinalReleaseComObject(comObject);
        }
    }
}

public sealed class SapiVoiceFactory : IVoiceFactory
{
    private readonly VoiceInfo _info;

    public SapiVoiceFactory(VoiceInfo info)
    {
        _info = info;
    }

    public VoiceInfo Info => _info;

    public IVoice Create()
    {
        return new SapiVoice(_info.DisplayName);
    }
}

public sealed class SapiVoiceFactoryProvider : IVoiceFactoryProvider
{
    private static readonly string[] VoiceCategoryIds =
    [
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices"
    ];

    public Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct)
    {
        if (!OperatingSystem.IsWindows())
        {
            return Task.FromResult<IReadOnlyList<IVoiceFactory>>([]);
        }

        ct.ThrowIfCancellationRequested();
        return Task.Run<IReadOnlyList<IVoiceFactory>>(ListVoicesWindows, ct);
    }

    [SupportedOSPlatform("windows")]
    private static IReadOnlyList<IVoiceFactory> ListVoicesWindows()
    {
        var results = new List<IVoiceFactory>();
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
                        var info = new VoiceInfo
                        {
                            Id = $"sapi:{name}",
                            DisplayName = name,
                            LocaleOrLanguage = locale,
                            ProviderName = "Windows SAPI"
                        };
                        results.Add(new SapiVoiceFactory(info));
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

        return results;
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
