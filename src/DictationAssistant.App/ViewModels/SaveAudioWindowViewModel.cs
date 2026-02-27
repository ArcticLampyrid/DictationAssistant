using CommunityToolkit.Mvvm.ComponentModel;
using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Lyric;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Audio.Encoder;
using DictationAssistant.App.Services;

namespace DictationAssistant.App.ViewModels;

public partial class SaveAudioWindowViewModel : ObservableObject
{
    private IDictationPlayer? _dictationPlayer;

    public event Action<string>? AlertRequested;

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

    public IReadOnlyList<string> LyricModeOptions { get; } = ["Dismiss", "Lrc File"];

    public IReadOnlyList<AudioEncoderInfo> EncoderOptions { get; }

    [ObservableProperty]
    private AudioEncoderInfo _selectedEncoder;

    [ObservableProperty]
    private string _channel = "Stereo";

    [ObservableProperty]
    private string _sampleFormat = "Signed 16bit";

    [ObservableProperty]
    private string _frequency = "44100";

    [ObservableProperty]
    private string _lyricMode = "Lrc File";

    [ObservableProperty]
    private string _targetPath = "dictation.wav";

    [ObservableProperty]
    private bool _isExporting;

    public SaveAudioWindowViewModel()
    {
        var encoders = new List<AudioEncoderInfo> { WaveEncoder.EncoderInfo };

        if (FFmpegAudioEncoder.IsFFmpegAvailableAsync().Result)
        {
            encoders.Add(new FFmpegAudioEncoderInfo("mp3"));
            encoders.Add(new FFmpegAudioEncoderInfo("opus"));
        }

        EncoderOptions = encoders;
        _selectedEncoder = encoders[0];
    }

    public void SetDictationPlayer(IDictationPlayer player)
    {
        _dictationPlayer = player;
    }

    public async Task<bool> ExportAsync(CancellationToken cancellationToken)
    {
        if (_dictationPlayer is null)
        {
            AlertRequested?.Invoke("错误：未初始化播放器");
            return false;
        }

        if (string.IsNullOrWhiteSpace(TargetPath))
        {
            AlertRequested?.Invoke("请指定输出路径");
            return false;
        }

        IsExporting = true;
        IProgress<double>? progress = null;

        try
        {
            var sampleRate = int.TryParse(Frequency, out var sr) ? sr : 44100;
            var channels = Channel == "Mono" ? 1 : 2;
            var sampleFormat = SampleFormat == "Unsigned 8bit" ? PcmSampleFormat.U8 : PcmSampleFormat.S16LE;
            var targetFormat = new PcmFormatInfo(sampleRate, channels, sampleFormat);

            var pcmAudio = SelectedEncoder.CreateEncoder(targetFormat, TargetPath);
            ILyricWriter? lyricWriter = null;

            if (LyricMode == "Lrc File")
            {
                var lrcPath = Path.ChangeExtension(TargetPath, "lrc");
                lyricWriter = new LyricWriter(lrcPath);
            }

            using (pcmAudio)
            {
                using var pcmWriter = new PcmWriter(targetFormat, pcmAudio.Data, leaveOpen: true);

                var exporter = new AudioExporter(
                    _dictationPlayer.Voice,
                    _dictationPlayer.WordListSource,
                    _dictationPlayer.WaitingTimeCalculator);
                await exporter.ExportAsync(
                    pcmWriter, lyricWriter,
                    _dictationPlayer.TimesPerWord, _dictationPlayer.Rate,
                    progress, cancellationToken).ConfigureAwait(false);

                if (pcmAudio.Data is FFmpegAudioEncoderStream ffmpegStream)
                {
                    await ffmpegStream.FinishAsync(cancellationToken).ConfigureAwait(false);
                }
            }

            IsExporting = false;
            AlertRequested?.Invoke("导出完成");
            return true;
        }
        catch (OperationCanceledException)
        {
            IsExporting = false;
            AlertRequested?.Invoke("导出已取消");
            return false;
        }
        catch (Exception ex)
        {
            IsExporting = false;
            AlertRequested?.Invoke($"导出失败: {ex.Message}");
            return false;
        }
    }

    partial void OnSelectedEncoderChanged(AudioEncoderInfo value)
    {
        if (string.IsNullOrWhiteSpace(TargetPath))
        {
            TargetPath = $"dictation.{value.Extension}";
            return;
        }

        TargetPath = Path.ChangeExtension(TargetPath, value.Extension) ?? TargetPath;
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
