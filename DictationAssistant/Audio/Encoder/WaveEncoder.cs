using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictationAssistant.Audio.Encoder
{
    public class WaveEncoder : Stream
    {
        private class WaveEncoderInfo : AudioEncoderInfo
        {
            public WaveEncoderInfo() : base("Waveform Audio", "wav")
            {
            }

            public override PcmStreamWithInfo CreateEncoder(PcmFormatInfo format, string path, object encodeSettings)
            {
                return new PcmStreamWithInfo(new WaveEncoder(format, path), format);
            }
        }

        public static AudioEncoderInfo EncoderInfo = new WaveEncoderInfo();
        private readonly long headerSize;
        protected Stream baseStream;
        private PcmFormatInfo pcmFormatInfo;

        public WaveEncoder(PcmFormatInfo pcmFormatInfo, string path)
        {
            this.pcmFormatInfo = pcmFormatInfo;
            baseStream = File.Open(path, FileMode.Create);
            PcmAudioHelper.WriteWaveHeader(baseStream, pcmFormatInfo, 0);
            headerSize = baseStream.Length;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush() => baseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => baseStream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                baseStream.Position = 0;
                PcmAudioHelper.WriteWaveHeader(baseStream, pcmFormatInfo, baseStream.Length - headerSize);
                baseStream.Dispose();
                baseStream = null;
            }
        }
    }
}
