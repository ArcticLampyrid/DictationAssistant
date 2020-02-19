using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictationAssistant.Lyric
{
    public interface ILyricWriter
    {
        void Append(string text, long offest);
        void Flush();
    }
}
