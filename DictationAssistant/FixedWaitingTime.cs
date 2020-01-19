using System;

namespace DictationAssistant
{
    public class FixedWaitingTime : IWaitingTimeCalculator
    {
        private readonly int Seconds;
        public FixedWaitingTime(int seconds)
        {
            this.Seconds = seconds;
        }
        public Int32 CalculateWaitingTime(string word)
        {
            return Seconds;
        }
    }
}
