using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DictationAssistant.Audio.Encoder
{
    public class ExternalAudioEncoder : Stream
    {
        private Process processObject = new Process();
        public ExternalAudioEncoder(PcmFormatInfo pcmFormatInfo, string path, string encoderFileName, string encoderArgumentsFormat)
        {
            processObject.StartInfo.FileName = encoderFileName;
            processObject.StartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, encoderArgumentsFormat, Path.GetFullPath(path));
            processObject.StartInfo.UseShellExecute = false;
            processObject.StartInfo.CreateNoWindow = true;
            processObject.StartInfo.RedirectStandardInput = true;
            processObject.Start();
            PcmAudioHelper.WriteWaveHeader(processObject.StandardInput.BaseStream, pcmFormatInfo, 0);
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush() => processObject.StandardInput.BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => processObject.StandardInput.BaseStream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                processObject.StandardInput.Close();
                processObject.WaitForExit();
                processObject.Dispose();
                processObject = null;
            }
        }
    }
}
