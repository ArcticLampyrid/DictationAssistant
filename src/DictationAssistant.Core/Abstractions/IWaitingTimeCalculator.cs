namespace DictationAssistant.Core.Abstractions;

public interface IWaitingTimeCalculator
{
    int CalculateWaitingTime(string word);
}
