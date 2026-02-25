namespace DictationAssistant.Core.Models;

public sealed class DictationSettings
{
    private int _intervalSeconds = 3;
    private int _timesPerWord = 2;
    private int _volume = 100;
    private int _rate;

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

    public int Volume
    {
        get => _volume;
        set => _volume = Math.Clamp(value, 0, 100);
    }

    public int Rate
    {
        get => _rate;
        set => _rate = Math.Clamp(value, -10, 10);
    }

    public string DefaultChineseVoiceName { get; set; } = string.Empty;

    public string DefaultEnglishVoiceName { get; set; } = string.Empty;
}
