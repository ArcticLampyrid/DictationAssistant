using CommunityToolkit.Mvvm.ComponentModel;

using DictationAssistant.App.Services.Settings;

namespace DictationAssistant.App.ViewModels;

public partial class PreferenceWindowViewModel : ObservableObject
{
    public PreferenceWindowViewModel()
        : this(new PreferenceSettings())
    {
    }

    public PreferenceWindowViewModel(PreferenceSettings settings)
    {
        EditorFontFamily = settings.EditorFontFamily;
        EditorFontSize = settings.EditorFontSize;
        DefaultChineseVoiceName = settings.DefaultChineseVoiceName;
        DefaultEnglishVoiceName = settings.DefaultEnglishVoiceName;
        ImprovedResourcePath = settings.ImprovedResourcePath;
    }

    public IReadOnlyList<string> VoiceOptions { get; } =
    [
        "默认语音（占位）",
        "中文语音（占位）",
        "英文语音（占位）"
    ];

    [ObservableProperty]
    private string _editorFontFamily = "Noto Sans CJK SC";

    [ObservableProperty]
    private double _editorFontSize = 28;

    [ObservableProperty]
    private string _defaultChineseVoiceName = "中文语音（占位）";

    [ObservableProperty]
    private string _defaultEnglishVoiceName = "英文语音（占位）";

    [ObservableProperty]
    private string _improvedResourcePath = string.Empty;

    public PreferenceSettings ToSettings()
    {
        return new PreferenceSettings
        {
            EditorFontFamily = EditorFontFamily,
            EditorFontSize = EditorFontSize,
            ImprovedResourcePath = ImprovedResourcePath,
            DefaultChineseVoiceName = DefaultChineseVoiceName,
            DefaultEnglishVoiceName = DefaultEnglishVoiceName
        };
    }
}
