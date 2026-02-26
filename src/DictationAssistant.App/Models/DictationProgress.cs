namespace DictationAssistant.Models;

public sealed record DictationProgress(
    int CurrentWordIndex,
    int CurrentRepeat,
    int TotalWords,
    bool IsCompleted)
{
    public static DictationProgress Empty => new(-1, 0, 0, false);

    public double Percent => TotalWords <= 0 ? 0 : Math.Clamp((double)(CurrentWordIndex + 1) / TotalWords, 0, 1);
}
