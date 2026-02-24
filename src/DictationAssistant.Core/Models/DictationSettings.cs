namespace DictationAssistant.Core.Models;

public sealed class DictationSettings
{
    private int _intervalSeconds = 3;
    private int _timesPerWord = 2;

    public int IntervalSeconds
    {
        get => _intervalSeconds;
        set => _intervalSeconds = Math.Clamp(value, 0, 600);
    }

    public int TimesPerWord
    {
        get => _timesPerWord;
        set => _timesPerWord = Math.Clamp(value, 1, 20);
    }

    public bool HighlightCurrentLine { get; set; } = true;

    public bool AutoScrollToCurrentLine { get; set; } = true;
}
