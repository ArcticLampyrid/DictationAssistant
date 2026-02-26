using CommunityToolkit.Mvvm.ComponentModel;
using DictationAssistant.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.Models;

namespace DictationAssistant.App.ViewModels;

public partial class SaveAudioWindowViewModel : ObservableObject
{
    private IDictationPlayer? _dictationPlayer;

    public IReadOnlyList<string> ChannelOptions { get; } = ["Mono", "Stereo"];

    public IReadOnlyList<string> SampleFormatOptions { get; } = ["Unsigned 8bit", "Signed 16bit"];

    public IReadOnlyList<string> FrequencyOptions { get; } =
    [
        "6000",
        "7333",
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

    [ObservableProperty]
    private bool _isExporting;

    public void SetDictationPlayer(IDictationPlayer player)
    {
        _dictationPlayer = player;
    }

    public async Task<bool> ExportAsync(CancellationToken cancellationToken)
    {
        if (_dictationPlayer is null)
        {
            Status = "错误：未初始化播放器";
            return false;
        }

        if (string.IsNullOrWhiteSpace(TargetPath))
        {
            Status = "请指定输出路径";
            return false;
        }

        IsExporting = true;
        Status = "正在导出...";

        var progress = new Progress<double>(p => Status = $"正在导出... {p:P0}");

        var request = new SaveAudioRequest
        {
            OutputPath = TargetPath,
            SampleRate = int.TryParse(Frequency, out var sr) ? sr : 44100,
            Channels = Channel == "Mono" ? 1 : 2,
            OutputFormat = OutputFormat,
            LyricMode = LyricMode,
            LyricsOutputPath = LyricMode == "Lrc File" ? Path.ChangeExtension(TargetPath, "lrc") : null,
            Progress = progress
        };

        var result = await _dictationPlayer.SaveAudioAsync(request, cancellationToken).ConfigureAwait(false);
        Status = result.Message;
        IsExporting = false;
        return result.Succeeded;
    }

    partial void OnOutputFormatChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(TargetPath))
        {
            TargetPath = $"dictation.{value}";
            return;
        }

        TargetPath = Path.ChangeExtension(TargetPath, value) ?? TargetPath;
    }

    public ILyricWriter? CreateLyricWriter()
    {
        if (LyricMode != "Lrc File" || string.IsNullOrWhiteSpace(TargetPath))
        {
            return null;
        }

        var lrcPath = Path.ChangeExtension(TargetPath, "lrc");
        return new LyricWriter(lrcPath);
    }
}
