namespace DictationAssistant.App.Services.Settings;

public sealed class AppSettings
{
    public MainWindowSettings MainWindow { get; set; } = new();

    public Core.Models.DictationSettings Dictation { get; set; } = new();

    public PreferenceSettings Preference { get; set; } = new();

    public void EnsureDefaults()
    {
        MainWindow ??= new MainWindowSettings();
        Dictation ??= new Core.Models.DictationSettings();
        Preference ??= new PreferenceSettings();
    }
}

public sealed class MainWindowSettings
{
    public double Width { get; set; } = 980;

    public double Height { get; set; } = 680;

    public double? X { get; set; }

    public double? Y { get; set; }

    public string WindowState { get; set; } = "Normal";

    public bool WordListVisible { get; set; } = true;
}

// App uses Core.Models.DictationSettings directly via using alias
// No separate App-layer DictationSettings needed

public sealed class PreferenceSettings
{
    public string EditorFontFamily { get; set; } = "Noto Sans CJK SC";

    public double EditorFontSize { get; set; } = 28;

    public string ImprovedResourcePath { get; set; } = string.Empty;

    public string DefaultChineseVoiceName { get; set; } = string.Empty;

    public string DefaultEnglishVoiceName { get; set; } = string.Empty;
}
