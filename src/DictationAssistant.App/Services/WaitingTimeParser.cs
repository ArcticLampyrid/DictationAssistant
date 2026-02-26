using DictationAssistant.App.Services;
using DictationAssistant.Abstractions;
using DictationAssistant.Services;

namespace DictationAssistant.App.Services;

public static class WaitingTimeParser
{
    public static IWaitingTimeCalculator Parse(string expression)
    {
        if (int.TryParse(expression.Trim(), out var secs))
            return new FixedWaitingTime(secs);
        return new ExpressionBasedWaitingTime(expression);
    }

    public static bool TryParse(string expression, out IWaitingTimeCalculator? calculator)
    {
        try
        {
            calculator = Parse(expression);
            return true;
        }
        catch
        {
            calculator = null;
            return false;
        }
    }
}
