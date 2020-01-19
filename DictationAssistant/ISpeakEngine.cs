using System.Globalization;

namespace DictationAssistant
{
    public class SpeakParam
    {
        /// <summary>
    /// 取值范围：-10 ~ 20
    /// </summary>
        public sbyte Rate = 0;
    }

    public interface ISpeakEngine
    {
        PcmStreamWithInfo Speak(string text, SpeakParam param);
        string Name { get; }
        CultureInfo Culture { get; }
    }
}
