using DictationAssistant.Core.Audio;
using ManagedBass;
using System.Diagnostics;
using System.IO;

namespace DictationAssistant.App.Services.Audio;

public static class BassAudioDecoder
{
    private static bool _initialized;
    private static readonly object _initLock = new();

    public static bool EnsureInitialized()
    {
        lock (_initLock)
        {
            if (!_initialized)
            {
                if (!Bass.Init(0))
                {
                    Trace.WriteLine($"[BassAudioDecoder] Failed to initialize BASS: {Bass.LastError}");
                    return false;
                }
                _initialized = true;
            }
            return true;
        }
    }

    public static PcmAudio? DecodeFile(string filePath)
    {
        var stream = CreateDecodeStream(filePath);
        if (stream == null)
        {
            return null;
        }

        try
        {
            return ReadPcmFromStream(stream);
        }
        finally
        {
            stream.Dispose();
        }
    }

    public static PcmAudio? DecodeStream(Stream inputStream)
    {
        var stream = CreateDecodeStream(inputStream);
        if (stream == null)
        {
            return null;
        }

        try
        {
            return ReadPcmFromStream(stream);
        }
        finally
        {
            stream.Dispose();
        }
    }

    public static BassDecodeStream? CreateDecodeStream(string filePath)
    {
        try
        {
            if (!EnsureInitialized())
            {
                return null;
            }

            return BassDecodeStream.CreateFromFile(filePath);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Exception creating stream for {filePath}: {ex.Message}");
            return null;
        }
    }

    public static BassDecodeStream? CreateDecodeStream(Stream inputStream)
    {
        try
        {
            return BassDecodeStream.CreateFromStream(inputStream);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Exception creating stream from Stream: {ex.Message}");
            return null;
        }
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
            Data = memoryStream.ToArray(),
            Format = format
        };
    }
}
