using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using DictationAssistant.App.Services;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly EditorDocumentWordListSource _wordListSource;
    private readonly IDictationPlayer _dictationPlayer;
    private readonly ITextFileService _textFileService;

    public MainWindowViewModel(
        EditorDocumentWordListSource wordListSource,
        IDictationPlayer dictationPlayer,
        ITextFileService textFileService,
        ITtsEngine ttsEngine)
    {
        _wordListSource = wordListSource;
        _dictationPlayer = dictationPlayer;
        _textFileService = textFileService;

        TtsEngineName = ttsEngine.Name;
        Status = "就绪";

        _dictationPlayer.ProgressChanged += (_, progress) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                CurrentLineIndex = progress.CurrentWordIndex;
                CurrentRepeat = progress.CurrentRepeat;

                ProgressText = progress.TotalWords <= 0
                    ? "0 / 0"
                    : $"{Math.Max(progress.CurrentWordIndex + 1, 0)} / {progress.TotalWords}";
                ProgressPercent = progress.Percent;
                OnPropertyChanged(nameof(SpeakStateText));
            });
        };

        _dictationPlayer.StateChanged += (_, state) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                CurrentState = state;
                OnPropertyChanged(nameof(IsAutoRunning));
                OnPropertyChanged(nameof(IsAutoPaused));
                OnPropertyChanged(nameof(PauseOrResumeAutoText));
                OnPropertyChanged(nameof(SpeakStateText));
            });
        };

        SyncSettingsFromCore();
        OnPropertyChanged(nameof(SpeakStateText));
    }

    public EditorDocumentWordListSource WordListSource => _wordListSource;

    [ObservableProperty]
    private string _ttsEngineName = string.Empty;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _status = string.Empty;

    [ObservableProperty]
    private int _currentLineIndex = -1;

    [ObservableProperty]
    private int _currentRepeat;

    [ObservableProperty]
    private DictationState _currentState;

    [ObservableProperty]
    private int _intervalSeconds = 3;

    [ObservableProperty]
    private int _timesPerWord = 2;

    [ObservableProperty]
    private bool _highlightCurrentLine = true;

    [ObservableProperty]
    private bool _autoScrollCurrentLine = true;

    [ObservableProperty]
    private string _progressText = "0 / 0";

    [ObservableProperty]
    private double _progressPercent;

    [ObservableProperty]
    private bool _wordListVisible = true;

    [ObservableProperty]
    private int _volume = 100;

    [ObservableProperty]
    private int _rate;

    public bool IsAutoRunning => CurrentState == DictationState.AutoRunning;

    public bool IsAutoPaused => CurrentState == DictationState.AutoPaused;

    public string ShowOrHideWordListText => WordListVisible ? "隐藏词语列表(_W)" : "显示词语列表(_W)";

    public string PauseOrResumeAutoText => IsAutoPaused ? "恢复自动播报(_R)" : "暂停自动播报(_P)";

    public string SpeakStateText
    {
        get
        {
            if (IsAutoPaused)
            {
                return "自动播报已暂停";
            }

            if (CurrentState == DictationState.ManualSpeaking)
            {
                var speakingIndex = Math.Max(CurrentLineIndex + 1, 1);
                return $"正在播报第{speakingIndex}个";
            }

            if (CurrentState == DictationState.AutoRunning)
            {
                return $"即将播报第{GetNextAutoIndex()}个";
            }

            return "等待播报";
        }
    }

    [RelayCommand]
    private void ToggleWordList()
    {
        WordListVisible = !WordListVisible;
        OnPropertyChanged(nameof(ShowOrHideWordListText));
    }

    [RelayCommand]
    private void PauseOrResumeAuto()
    {
        if (IsAutoPaused)
        {
            ResumeAuto();
            return;
        }

        PauseAuto();
    }

    public async Task<string?> LoadTextFromFileAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Status = "请选择文件";
            return null;
        }

        var content = await _textFileService.ReadAllTextAsync(path).ConfigureAwait(false);
        FilePath = path;
        Status = $"已加载：{path}";
        ResetProgressUi();
        return content;
    }

    public async Task SaveTextToFileAsync(string path, string text)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Status = "请选择保存路径";
            return;
        }

        await _textFileService.WriteAllTextAsync(path, text).ConfigureAwait(false);
        FilePath = path;
        Status = $"已保存：{path}";
    }

    [RelayCommand]
    private async Task SpeakPreviousAsync()
    {
        ApplySettingsToCore();
        await _dictationPlayer.SpeakPreviousAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task SpeakAgainAsync()
    {
        ApplySettingsToCore();
        await _dictationPlayer.SpeakAgainAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task SpeakNextAsync()
    {
        ApplySettingsToCore();
        await _dictationPlayer.SpeakNextAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task StartAutoAsync()
    {
        ApplySettingsToCore();
        var startIndex = CurrentLineIndex >= 0 ? CurrentLineIndex : 0;
        await _dictationPlayer.StartAutoAsync(startIndex).ConfigureAwait(false);
    }

    [RelayCommand]
    private void PauseAuto()
    {
        _dictationPlayer.PauseAuto();
    }

    [RelayCommand]
    private void ResumeAuto()
    {
        _dictationPlayer.ResumeAuto();
    }

    [RelayCommand]
    private async Task StopAsync()
    {
        await _dictationPlayer.StopAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task SaveAudioAsync()
    {
        var request = new SaveAudioRequest
        {
            OutputPath = string.IsNullOrWhiteSpace(FilePath) ? "dictation.wav" : FilePath + ".wav"
        };
        var result = await _dictationPlayer.SaveAudioAsync(request).ConfigureAwait(false);
        Status = result.Message;
    }

    public async Task SpeakLineAsync(int index)
    {
        CurrentLineIndex = index;
        ApplySettingsToCore();
        await _dictationPlayer.SpeakAtAsync(index).ConfigureAwait(false);
    }

    public async Task StartAutoFromLineAsync(int index)
    {
        CurrentLineIndex = index;
        ApplySettingsToCore();
        await _dictationPlayer.StartAutoAsync(index).ConfigureAwait(false);
    }

    private void ApplySettingsToCore()
    {
        _dictationPlayer.Settings.IntervalSeconds = IntervalSeconds;
        _dictationPlayer.Settings.TimesPerWord = TimesPerWord;
        _dictationPlayer.Settings.HighlightCurrentLine = HighlightCurrentLine;
        _dictationPlayer.Settings.AutoScrollToCurrentLine = AutoScrollCurrentLine;
    }

    private void SyncSettingsFromCore()
    {
        IntervalSeconds = _dictationPlayer.Settings.IntervalSeconds;
        TimesPerWord = _dictationPlayer.Settings.TimesPerWord;
        HighlightCurrentLine = _dictationPlayer.Settings.HighlightCurrentLine;
        AutoScrollCurrentLine = _dictationPlayer.Settings.AutoScrollToCurrentLine;
    }

    private void ResetProgressUi()
    {
        CurrentLineIndex = -1;
        CurrentRepeat = 0;
        ProgressText = "0 / 0";
        ProgressPercent = 0;
        OnPropertyChanged(nameof(SpeakStateText));
    }

    partial void OnWordListVisibleChanged(bool value)
    {
        _ = value;
        OnPropertyChanged(nameof(ShowOrHideWordListText));
    }

    private int GetNextAutoIndex()
    {
        var total = _wordListSource.Count;
        if (total <= 0)
        {
            return 1;
        }

        var current = Math.Max(CurrentLineIndex, 0);
        var next = CurrentRepeat >= TimesPerWord ? Math.Min(current + 1, total - 1) : current;
        return next + 1;
    }
}
