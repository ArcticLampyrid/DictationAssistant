namespace DictationAssistant.App.Settings;

public sealed class AppSettings
{
    public MainWindowSettings MainWindow { get; set; } = new();

    public DictationAssistant.App.Models.DictationSettings Dictation { get; set; } = new();

    public PreferenceSettings Preference { get; set; } = new();

    public void EnsureDefaults()
    {
        MainWindow ??= new MainWindowSettings();
        Dictation ??= new DictationAssistant.App.Models.DictationSettings();
        Preference ??= new PreferenceSettings();
    }
}

public sealed class MainWindowSettings
{
}

// App uses Core.Models.DictationSettings directly via using alias
// No separate App-layer DictationSettings needed

public sealed class PreferenceSettings
{
    public string EditorFontFamily { get; set; } = "Noto Sans CJK SC";

    public double EditorFontSize { get; set; } = 28;

    public string ImprovedResourcePath { get; set; } = string.Empty;

    public string DefaultChineseVoiceId { get; set; } = string.Empty;

    public string DefaultEnglishVoiceId { get; set; } = string.Empty;
}
