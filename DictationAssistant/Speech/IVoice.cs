using DictationAssistant.Audio;
using System.Globalization;

namespace DictationAssistant.Speech
{
    public class SpeakParam
    {
        /// <summary>
    /// 取值范围：-10 ~ 20
    /// </summary>
        public sbyte Rate = 0;
    }

    public interface IVoice
    {
        PcmStreamWithInfo Speak(string text, SpeakParam param);
        string Name { get; }
        CultureInfo Culture { get; }
    }
}
