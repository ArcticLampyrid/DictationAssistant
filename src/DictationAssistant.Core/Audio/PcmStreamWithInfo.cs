using System;
using System.IO;

namespace DictationAssistant.Core.Audio;

public sealed class PcmStreamWithInfo : IDisposable
{
    public static PcmStreamWithInfo Empty = new PcmStreamWithInfo(Stream.Null, new PcmFormatInfo(44100, 2, PcmSampleFormat.S16LE));

    public Stream PcmStream { get; }
    public PcmFormatInfo Format { get; }

    public PcmStreamWithInfo(Stream pcmStream, PcmFormatInfo format)
    {
        PcmStream = pcmStream;
        Format = format;
    }

    public void Dispose()
    {
        PcmStream.Dispose();
    }
}
