using DictationAssistant.Audio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DictationAssistant.Speech
{
    public class NullVoice : IVoice
    {
        public static NullVoice Instance { get; } = new NullVoice();
        private NullVoice()
        {
        }

        public string Name => "Null Voice";

        public CultureInfo Culture => CultureInfo.InvariantCulture;

        public PcmStreamWithInfo Speak(string text, SpeakParam param)
        {
            return PcmStreamWithInfo.Empty;
        }
    }
}
