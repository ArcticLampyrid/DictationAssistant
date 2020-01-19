using System;

namespace DictationAssistant
{
    public interface IWaitingTimeCalculator
    {
        int CalculateWaitingTime(string word);
    }
}
