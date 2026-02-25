namespace DictationAssistant.App.Services.Settings;

public sealed class AppSettings
{
    public MainWindowSettings MainWindow { get; set; } = new();

    public DictationSettings Dictation { get; set; } = new();

    public PreferenceSettings Preference { get; set; } = new();

    public void EnsureDefaults()
    {
        MainWindow ??= new MainWindowSettings();
        Dictation ??= new DictationSettings();
        Preference ??= new PreferenceSettings();
    }
}

public sealed class MainWindowSettings
{
    public double Width { get; set; } = 980;

    public double Height { get; set; } = 680;

    public bool WordListVisible { get; set; } = true;
}

public sealed class DictationSettings
{
    public int IntervalSeconds { get; set; } = 3;

    public int TimesPerWord { get; set; } = 2;

    public bool HighlightCurrentLine { get; set; } = true;

    public bool AutoScrollCurrentLine { get; set; } = true;

    public int Volume { get; set; } = 100;

    public int Rate { get; set; }
}

public sealed class PreferenceSettings
{
    public string EditorFontFamily { get; set; } = "Noto Sans CJK SC";

    public double EditorFontSize { get; set; } = 28;

    public string ImprovedResourcePath { get; set; } = string.Empty;

    public string DefaultChineseVoiceName { get; set; } = string.Empty;

    public string DefaultEnglishVoiceName { get; set; } = string.Empty;
}
