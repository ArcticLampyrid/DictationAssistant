using DictationAssistant.App.Services.Audio;
using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Models;
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
            using var decodeStream = BassAudioDecoder.DecodeFile(filePath);
            if (decodeStream is not null)
            {
                return ReadPcmFromStream(decodeStream);
            }
        }

        return await _inner.SynthesizePcmAsync(text, options, ct).ConfigureAwait(false);
    }

    private static PcmAudio? ReadPcmFromStream(BassDecodeStream decodeStream)
    {
        var format = decodeStream.Format;

        using var memoryStream = new MemoryStream();
        var buffer = new byte[8192];
        int bytesRead;

        while ((bytesRead = decodeStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, bytesRead);
        }

        return new PcmAudio
        {
            Data = memoryStream,
            Format = format
        };
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
