using CommunityToolkit.Mvvm.ComponentModel;

namespace DictationAssistant.App.ViewModels;

public partial class SaveAudioWindowViewModel : ObservableObject
{
    public IReadOnlyList<string> ChannelOptions { get; } = ["Mono", "Stereo"];

    public IReadOnlyList<string> SampleFormatOptions { get; } = ["Unsigned 8bit", "Signed 16bit"];

    public IReadOnlyList<string> FrequencyOptions { get; } =
    [
        "8000",
        "11025",
        "16000",
        "22050",
        "24000",
        "32000",
        "44100",
        "48000"
    ];

    public IReadOnlyList<string> OutputFormatOptions { get; } = ["wav", "mp3", "opus"];

    public IReadOnlyList<string> LyricModeOptions { get; } = ["Dismiss", "Lrc File"];

    [ObservableProperty]
    private string _channel = "Stereo";

    [ObservableProperty]
    private string _sampleFormat = "Signed 16bit";

    [ObservableProperty]
    private string _frequency = "44100";

    [ObservableProperty]
    private string _outputFormat = "wav";

    [ObservableProperty]
    private string _lyricMode = "Lrc File";

    [ObservableProperty]
    private string _targetPath = "dictation.wav";

    [ObservableProperty]
    private string _status = string.Empty;

    partial void OnOutputFormatChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(TargetPath))
        {
            TargetPath = $"dictation.{value}";
            return;
        }

        TargetPath = Path.ChangeExtension(TargetPath, value) ?? TargetPath;
    }
}
