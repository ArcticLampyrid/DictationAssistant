using DictationAssistant.App.Audio;
using System;
using System.IO;

namespace DictationAssistant.App.Services.Audio;

public static class WavWriterExtensions
{
    public static void Write(Stream output, PcmAudio audio, int targetSampleRate, int targetChannels)
    {
        using var tempStream = new MemoryStream();
        SdlAudioConverter.WriteWithResample(tempStream, audio, targetSampleRate, targetChannels);

        var dataLength = (int)tempStream.Length;
        WavWriter.WriteHeader(output, targetSampleRate, targetChannels, dataLength);

        tempStream.Position = 0;
        tempStream.CopyTo(output);
    }
}
