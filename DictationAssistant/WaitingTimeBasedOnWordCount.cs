using System;

namespace DictationAssistant
{
    public class WaitingTimeBasedOnWordCount : IWaitingTimeCalculator
    {
        private Int32 SecondsPerWord;
        private Int32 BasedSecond;
        public WaitingTimeBasedOnWordCount(int secondsPerWord, int basedSecond = 0)
        {
            this.SecondsPerWord = secondsPerWord;
            this.BasedSecond = basedSecond;
        }
        public Int32 CalculateWaitingTime(string word)
        {
            return SecondsPerWord != 0 ? WaitingTimeHelper.GetWordCount(word) * SecondsPerWord : 0 + BasedSecond;
        }
    }
}
