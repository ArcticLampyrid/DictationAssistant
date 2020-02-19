using System;
using System.IO;

namespace DictationAssistant.Audio
{
    public sealed class PcmStreamWithInfo : IDisposable
    {
        public static PcmStreamWithInfo Empty = new PcmStreamWithInfo(Stream.Null, new PcmFormatInfo(44100, 2, PcmSampleFormat.S16));
        public Stream PcmStream { get; }
        public PcmFormatInfo Format { get; }
        public PcmStreamWithInfo(Stream PcmStream, PcmFormatInfo Format)
        {
            this.PcmStream = PcmStream;
            this.Format = Format;
        }
        public void Dispose()
        {
            PcmStream.Dispose();
        }
    }
}
