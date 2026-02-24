using CommunityToolkit.Mvvm.ComponentModel;

namespace DictationAssistant.App.ViewModels;

public partial class PreferenceWindowViewModel : ObservableObject
{
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
    private string _defaultChineseVoice = "中文语音（占位）";

    [ObservableProperty]
    private string _defaultEnglishVoice = "英文语音（占位）";

    [ObservableProperty]
    private string _improvedResourcePath = string.Empty;
}
