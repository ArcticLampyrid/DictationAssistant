using DictationAssistant.Core.Audio;
using ManagedBass;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public static class BassAudioDecoder
{
    private static bool _initialized;
    private static readonly object _initLock = new();

    private static bool EnsureInitialized()
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
        try
        {
            if (!EnsureInitialized())
            {
                return null;
            }

            var stream = Bass.CreateStream(filePath, Flags: BassFlags.Decode | BassFlags.Unicode);
            if (stream == 0)
            {
                Trace.WriteLine($"[BassAudioDecoder] Failed to create stream for {filePath}: {Bass.LastError}");
                return null;
            }

            try
            {
                return ReadPcmFromStream(stream);
            }
            finally
            {
                Bass.StreamFree(stream);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Exception decoding {filePath}: {ex.Message}");
            return null;
        }
    }

    public static PcmAudio? DecodeStream(Stream inputStream)
    {
        if (!EnsureInitialized())
        {
            return null;
        }

        // Create FileProcedures with proper signatures for BASS custom stream
        var fileProcs = new FileProcedures
        {
            Close = user => { },
            Length = _ => inputStream.Length,
            Read = (buffer, length, user) =>
            {
                var ptr = buffer;
                var bytes = new byte[length];
                var bytesRead = inputStream.Read(bytes, 0, length);
                if (bytesRead > 0)
                {
                    Marshal.Copy(bytes, 0, ptr, bytesRead);
                }
                return bytesRead;
            },
            Seek = (offset, user) =>
            {
                try
                {
                    inputStream.Seek(offset, SeekOrigin.Begin);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        };

        var stream = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Decode | BassFlags.Unicode, fileProcs);
        if (stream == 0)
        {
            Trace.WriteLine($"[BassAudioDecoder] Failed to create stream from custom stream: {Bass.LastError}");
            return null;
        }

        try
        {
            return ReadPcmFromStream(stream);
        }
        finally
        {
            Bass.StreamFree(stream);
        }
    }

    private static PcmAudio? ReadPcmFromStream(int stream)
    {
        var channelInfo = Bass.ChannelGetInfo(stream);
        var sampleRate = channelInfo.Frequency;
        var channels = channelInfo.Channels;

        var format = new PcmFormatInfo(sampleRate, channels, PcmSampleFormat.S16LE);

        using var memoryStream = new MemoryStream();
        var buffer = new byte[8192];
        int bytesRead;

        while ((bytesRead = Bass.ChannelGetData(stream, buffer, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, bytesRead);
        }

        if (bytesRead < 0 && Bass.LastError != Errors.Ended)
        {
            Trace.WriteLine($"[BassAudioDecoder] Error reading channel data: {Bass.LastError}");
            return null;
        }

        return new PcmAudio
        {
            Data = memoryStream.ToArray(),
            Format = format
        };
    }
}
