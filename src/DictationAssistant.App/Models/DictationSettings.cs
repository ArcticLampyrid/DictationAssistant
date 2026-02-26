namespace DictationAssistant.App.Models;

public sealed class DictationSettings
{
    private string _intervalExpression = "3";
    private int _timesPerWord = 2;
    private int _volume = 100;
    private int _rate;

    public string IntervalExpression
    {
        get => _intervalExpression;
        set => _intervalExpression = string.IsNullOrWhiteSpace(value) ? "3" : value;
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
