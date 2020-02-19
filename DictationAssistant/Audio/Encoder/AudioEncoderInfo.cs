using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictationAssistant.Audio.Encoder
{
    public abstract class AudioEncoderInfo
    {
        protected AudioEncoderInfo(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        public string Name { get; }
        public string Extension { get; }

        public abstract PcmStreamWithInfo CreateEncoder(PcmFormatInfo format, string path, object encodeSettings);
        public PcmStreamWithInfo CreateEncoder(PcmFormatInfo format, string path)
        {
            return CreateEncoder(format, path, null);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
