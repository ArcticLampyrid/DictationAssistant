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

    [ObservableProperty]
    private double _exportProgress;

    private CancellationTokenSource? _exportCts;

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

    public async Task<bool> ExportAsync()
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

        // Snapshot all parameters from UI thread before going async
        var words = _dictationPlayer.WordListSource.GetWords();
        var voice = _dictationPlayer.Voice;
        var waitingTimeCalculator = _dictationPlayer.WaitingTimeCalculator;
        var timesPerWord = _dictationPlayer.TimesPerWord;
        var rate = _dictationPlayer.Rate;
        var sampleRate = int.TryParse(Frequency, out var sr) ? sr : 44100;
        var channels = Channel == "Mono" ? 1 : 2;
        var sampleFormat = SampleFormat == "Unsigned 8bit" ? PcmSampleFormat.U8 : PcmSampleFormat.S16LE;
        var targetFormat = new PcmFormatInfo(sampleRate, channels, sampleFormat);
        var selectedEncoder = SelectedEncoder;
        var targetPath = TargetPath;
        var lyricMode = LyricMode;

        IsExporting = true;
        ExportProgress = 0;
        _exportCts = new CancellationTokenSource();
        var ct = _exportCts.Token;

        var progress = new Progress<double>(p => ExportProgress = p);

        try
        {
            await Task.Run(async () =>
            {
                var encoderAudio = selectedEncoder.CreateEncoder(targetFormat, targetPath);
                ILyricWriter? lyricWriter = null;

                if (lyricMode == "Lrc File")
                {
                    var lrcPath = Path.ChangeExtension(targetPath, "lrc");
                    lyricWriter = new LyricWriter(lrcPath);
                }

                using (encoderAudio)
                {
                    using var pcmWriter = new PcmWriter(targetFormat, encoderAudio.Data, leaveOpen: true);

                    var exporter = new AudioExporter(voice, words, waitingTimeCalculator);
                    await exporter.ExportAsync(
                        pcmWriter, lyricWriter,
                        timesPerWord, rate,
                        progress, ct).ConfigureAwait(false);

                    if (encoderAudio.Data is FFmpegAudioEncoderStream ffmpegStream)
                    {
                        await ffmpegStream.FinishAsync(ct).ConfigureAwait(false);
                    }
                }
            }, ct).ConfigureAwait(false);

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
        finally
        {
            _exportCts?.Dispose();
            _exportCts = null;
        }
    }

    public void CancelExport()
    {
        _exportCts?.Cancel();
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
