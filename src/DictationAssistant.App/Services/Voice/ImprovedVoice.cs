using DictationAssistant.App.Services.Audio;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;
using System.IO;

namespace DictationAssistant.App.Services.Voice;

public sealed class ImprovedVoice : IVoice
{
    private static readonly string[] AudioFileExtensions = ["wav", "flac", "ape", "m4a", "opus", "aac", "mp3", "mp2", "mp1", "ogg", "wma", "aif", "mp4"];

    private readonly IVoice _inner;
    private readonly string _resourceDirectory;

    public ImprovedVoice(IVoice inner, string resourceDirectory)
    {
        _inner = inner;
        _resourceDirectory = resourceDirectory;
    }

    public string Name => $"[Improved] {_inner.Name}";

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
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

        return await _inner.SynthesizePcmAsync(text, options, ct).ConfigureAwait(false);
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
