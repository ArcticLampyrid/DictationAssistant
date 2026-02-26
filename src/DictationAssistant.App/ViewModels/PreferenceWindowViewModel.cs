using CommunityToolkit.Mvvm.ComponentModel;

using DictationAssistant.App.Settings;

namespace DictationAssistant.App.ViewModels;

public partial class PreferenceWindowViewModel : ObservableObject
{
    public PreferenceWindowViewModel()
        : this(new PreferenceSettings(), [])
    {
    }

    public PreferenceWindowViewModel(PreferenceSettings settings, IReadOnlyList<string> voiceOptions)
    {
        EditorFontFamily = settings.EditorFontFamily;
        EditorFontSize = settings.EditorFontSize;
        DefaultChineseVoiceName = settings.DefaultChineseVoiceName;
        DefaultEnglishVoiceName = settings.DefaultEnglishVoiceName;
        ImprovedResourcePath = settings.ImprovedResourcePath;

        VoiceOptions = voiceOptions.Count > 0
            ? voiceOptions.ToArray()
            : ["（当前引擎不支持枚举语音）"];
    }

    public IReadOnlyList<string> VoiceOptions { get; }

    [ObservableProperty]
    private string _editorFontFamily = "Noto Sans CJK SC";

    [ObservableProperty]
    private double _editorFontSize = 28;

    [ObservableProperty]
    private string _defaultChineseVoiceName = string.Empty;

    [ObservableProperty]
    private string _defaultEnglishVoiceName = string.Empty;

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
