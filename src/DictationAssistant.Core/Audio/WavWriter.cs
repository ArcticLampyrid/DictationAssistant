using System;

namespace DictationAssistant.Core.Audio;

public static class WavWriter
{
    private static readonly byte[] Byte00_1M = new byte[1048576];
    private static readonly byte[] Byte80_1M = new byte[1048576];

    static WavWriter()
    {
        Array.Fill(Byte80_1M, (byte)0x80);
    }

    public static long WriteDelay(Stream output, PcmFormatInfo format, long ms)
    {
        var sampleRate = format.SampleRate;
        var channels = format.Channels;
        var sampleFormat = format.SampleFormat;
        var bytesPerSample = sampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var blockAlign = channels * bytesPerSample;

        var totalSamples = (ms / 1000) * sampleRate + ((ms % 1000) * sampleRate) / 1000;
        var totalBytes = totalSamples * blockAlign;

        var emptyData = sampleFormat == PcmSampleFormat.U8 ? Byte80_1M : Byte00_1M;
        var remainingBytes = (int)totalBytes;

        while (remainingBytes > 0)
        {
            var bytesToWrite = Math.Min(remainingBytes, emptyData.Length);
            output.Write(emptyData, 0, bytesToWrite);
            remainingBytes -= bytesToWrite;
        }

        return totalBytes;
    }

    public static void Write(Stream output, PcmAudio audio)
    {
        var dataLength = (int)audio.Data.Length;
        WriteHeader(output, audio.Format.SampleRate, audio.Format.Channels, dataLength);
        audio.Data.Position = 0;
        audio.Data.CopyTo(output);
    }

    public static void WriteHeader(Stream output, int sampleRate, int channels, int dataLength)
    {
        var byteRate = sampleRate * channels * 2;
        var blockAlign = channels * 2;

        using var writer = new BinaryWriter(output, System.Text.Encoding.UTF8, leaveOpen: true);

        writer.Write("RIFF"u8.ToArray());
        writer.Write(36 + dataLength);
        writer.Write("WAVE"u8.ToArray());

        writer.Write("fmt "u8.ToArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)channels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write((short)16);

        writer.Write("data"u8.ToArray());
        writer.Write(dataLength);
    }
}
