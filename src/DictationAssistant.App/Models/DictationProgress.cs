namespace DictationAssistant.App.Models;

public sealed record DictationProgress(
    int CurrentWordIndex,
    int CurrentRepeat,
    int TotalWords,
    int? NextWordIndex,
    DateTimeOffset? NextSpeakTime,
    bool IsCompleted)
{
    public static DictationProgress Empty => new(-1, 0, 0, null, null, false);

    public double Percent => TotalWords <= 0 ? 0 : Math.Clamp((double)(CurrentWordIndex + 1) / TotalWords, 0, 1);
}
