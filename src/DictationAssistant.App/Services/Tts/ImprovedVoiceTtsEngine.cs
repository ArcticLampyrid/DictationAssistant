using DictationAssistant.App.Services.Audio;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using System.IO;

namespace DictationAssistant.App.Services.Tts;

public sealed class ImprovedVoiceTtsEngine : IPcmTtsEngine
{
    private static readonly string[] AudioFileExtensions = ["wav", "flac", "ape", "m4a", "opus", "aac", "mp3", "mp2", "mp1", "ogg", "wma", "aif", "mp4"];

    private readonly IPcmTtsEngine _fallbackEngine;
    private readonly string _resourceDirectory;

    public ImprovedVoiceTtsEngine(IPcmTtsEngine fallbackEngine, string resourceDirectory)
    {
        _fallbackEngine = fallbackEngine;
        _resourceDirectory = resourceDirectory;
    }

    public string Name => $"[Improved] {_fallbackEngine.Name}";

    public Task<IReadOnlyList<TtsVoiceInfo>> ListVoicesAsync(CancellationToken ct)
    {
        return _fallbackEngine.ListVoicesAsync(ct);
    }

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, TtsSpeakOptions options, CancellationToken ct)
    {
        var filePath = FindFile(text);
        if (filePath is not null)
        {
            var pcmAudio = BassAudioDecoder.DecodeFile(filePath);
            if (pcmAudio is not null)
            {
                return pcmAudio;
            }
        }

        return await _fallbackEngine.SynthesizePcmAsync(text, options, ct).ConfigureAwait(false);
    }

    private string? FindFile(string text)
    {
        var sanitized = text
            .Replace("\\", "")
            .Replace("/", "")
            .Replace(":", "")
            .Replace("*", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        var basePath = Path.Combine(_resourceDirectory, sanitized);

        foreach (var ext in AudioFileExtensions)
        {
            var fullPath = basePath + "." + ext;
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }
}
