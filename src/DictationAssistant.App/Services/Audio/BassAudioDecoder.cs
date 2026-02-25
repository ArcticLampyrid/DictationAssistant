using DictationAssistant.Core.Audio;
using ManagedBass;
using System.Diagnostics;
using System.IO;

namespace DictationAssistant.App.Services.Audio;

public static class BassAudioDecoder
{
    private static bool _initialized;
    private static readonly object _initLock = new();

    public static PcmAudio? DecodeFile(string filePath)
    {
        try
        {
            lock (_initLock)
            {
                if (!_initialized)
                {
                    if (!Bass.Init(0))
                    {
                        Trace.WriteLine($"[BassAudioDecoder] Failed to initialize BASS: {Bass.LastError}");
                        return null;
                    }
                    _initialized = true;
                }
            }

            var stream = Bass.CreateStream(filePath, Flags: BassFlags.Decode | BassFlags.Unicode);
            if (stream == 0)
            {
                Trace.WriteLine($"[BassAudioDecoder] Failed to create stream for {filePath}: {Bass.LastError}");
                return null;
            }

            try
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

                if (bytesRead < 0)
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
}
