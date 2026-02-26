namespace DictationAssistant.Abstractions;

public interface IWaitingTimeCalculator
{
    int CalculateWaitingTime(string word);
}
