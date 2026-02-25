using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using System.Diagnostics;
using DictationAssistant.App.Services;
using DictationAssistant.App.Services.Settings;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;
using DictationAssistant.Core.Services;

namespace DictationAssistant.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly EditorDocumentWordListSource _wordListSource;
    private readonly IDictationPlayer _dictationPlayer;
    private readonly ITextFileService _textFileService;
    private readonly AppSettings _appSettings;

    public IDictationPlayer DictationPlayer => _dictationPlayer;

    public MainWindowViewModel(
        EditorDocumentWordListSource wordListSource,
        IDictationPlayer dictationPlayer,
        ITextFileService textFileService,
        IPcmTtsEngine ttsEngine,
        AppSettings appSettings)
    {
        _wordListSource = wordListSource;
        _dictationPlayer = dictationPlayer;
        _textFileService = textFileService;
        _appSettings = appSettings;
        _appSettings.EnsureDefaults();

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

        LoadSettings();
        ApplySettingsToCore();
        _ = LoadVoiceOptionsAsync(ttsEngine);
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
    private string _intervalExpression = "3";

    [ObservableProperty]
    private string _intervalValidationHint = string.Empty;

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

    [ObservableProperty]
    private double _mainWindowWidth = 980;

    [ObservableProperty]
    private double _mainWindowHeight = 680;

    [ObservableProperty]
    private string _editorFontFamily = "Noto Sans CJK SC";

    [ObservableProperty]
    private double _editorFontSize = 28;

    [ObservableProperty]
    private string _improvedResourcePath = string.Empty;

    [ObservableProperty]
    private string _defaultChineseVoiceName = string.Empty;

    [ObservableProperty]
    private string _defaultEnglishVoiceName = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TtsVoiceInfo> _voiceOptions = [];

    [ObservableProperty]
    private TtsVoiceInfo? _selectedVoice;

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
        _dictationPlayer.Settings.IntervalExpression = IntervalExpression;
        _dictationPlayer.Settings.TimesPerWord = TimesPerWord;
        _dictationPlayer.Settings.HighlightCurrentLine = HighlightCurrentLine;
        _dictationPlayer.Settings.AutoScrollToCurrentLine = AutoScrollCurrentLine;
        _dictationPlayer.Settings.Volume = Volume;
        _dictationPlayer.Settings.Rate = Rate;
        _dictationPlayer.Settings.DefaultChineseVoiceName = DefaultChineseVoiceName;
        _dictationPlayer.Settings.DefaultEnglishVoiceName = DefaultEnglishVoiceName;
    }

    private async Task LoadVoiceOptionsAsync(IPcmTtsEngine ttsEngine)
    {
        try
        {
            var voices = await ttsEngine.ListVoicesAsync(CancellationToken.None).ConfigureAwait(false);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                VoiceOptions.Clear();
                foreach (var voice in voices)
                {
                    if (!string.IsNullOrWhiteSpace(voice.Name))
                    {
                        VoiceOptions.Add(voice);
                    }
                }

                var targetVoice = DefaultChineseVoiceName ?? DefaultEnglishVoiceName;
                if (!string.IsNullOrWhiteSpace(targetVoice))
                {
                    SelectedVoice = VoiceOptions.FirstOrDefault(v => v.Name == targetVoice);
                }
            });
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Failed to load TTS voices: {ex}");
            await Dispatcher.UIThread.InvokeAsync(() => VoiceOptions.Clear());
        }
    }

    partial void OnSelectedVoiceChanged(TtsVoiceInfo? value)
    {
        if (value != null)
        {
            DefaultChineseVoiceName = value.Name;
            DefaultEnglishVoiceName = value.Name;
        }
    }

    public PreferenceSettings CreatePreferenceSnapshot()
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

    public void ApplyPreferenceSettings(PreferenceSettings settings)
    {
        EditorFontFamily = settings.EditorFontFamily;
        EditorFontSize = settings.EditorFontSize;
        ImprovedResourcePath = settings.ImprovedResourcePath;
        DefaultChineseVoiceName = settings.DefaultChineseVoiceName;
        DefaultEnglishVoiceName = settings.DefaultEnglishVoiceName;
    }

    private void LoadSettings()
    {
        MainWindowWidth = _appSettings.MainWindow.Width;
        MainWindowHeight = _appSettings.MainWindow.Height;
        WordListVisible = _appSettings.MainWindow.WordListVisible;

        IntervalExpression = _appSettings.Dictation.IntervalExpression;
        TimesPerWord = _appSettings.Dictation.TimesPerWord;
        HighlightCurrentLine = _appSettings.Dictation.HighlightCurrentLine;
        AutoScrollCurrentLine = _appSettings.Dictation.AutoScrollCurrentLine;
        Volume = _appSettings.Dictation.Volume;
        Rate = _appSettings.Dictation.Rate;

        EditorFontFamily = _appSettings.Preference.EditorFontFamily;
        EditorFontSize = _appSettings.Preference.EditorFontSize;
        ImprovedResourcePath = _appSettings.Preference.ImprovedResourcePath;
        DefaultChineseVoiceName = _appSettings.Preference.DefaultChineseVoiceName;
        DefaultEnglishVoiceName = _appSettings.Preference.DefaultEnglishVoiceName;
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
        _appSettings.MainWindow.WordListVisible = value;
        OnPropertyChanged(nameof(ShowOrHideWordListText));
    }

    partial void OnIntervalExpressionChanged(string value)
    {
        _appSettings.Dictation.IntervalExpression = value;
        ValidateAndApplyWaitingTime(value);
    }

    private void ValidateAndApplyWaitingTime(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            IntervalValidationHint = "表达式不能为空";
            return;
        }

        if (WaitingTimeParser.TryParse(expression, out var calculator))
        {
            IntervalValidationHint = string.Empty;
            if (_dictationPlayer is DictationPlayer player)
            {
                player.SetWaitingTimeCalculator(calculator);
            }
        }
        else
        {
            IntervalValidationHint = "无效的表达式";
        }
    }

    partial void OnTimesPerWordChanged(int value)
    {
        _appSettings.Dictation.TimesPerWord = value;
    }

    partial void OnHighlightCurrentLineChanged(bool value)
    {
        _appSettings.Dictation.HighlightCurrentLine = value;
    }

    partial void OnAutoScrollCurrentLineChanged(bool value)
    {
        _appSettings.Dictation.AutoScrollCurrentLine = value;
    }

    partial void OnVolumeChanged(int value)
    {
        _appSettings.Dictation.Volume = value;
    }

    partial void OnRateChanged(int value)
    {
        _appSettings.Dictation.Rate = value;
    }

    partial void OnMainWindowWidthChanged(double value)
    {
        _appSettings.MainWindow.Width = value;
    }

    partial void OnMainWindowHeightChanged(double value)
    {
        _appSettings.MainWindow.Height = value;
    }

    partial void OnEditorFontFamilyChanged(string value)
    {
        _appSettings.Preference.EditorFontFamily = value;
    }

    partial void OnEditorFontSizeChanged(double value)
    {
        _appSettings.Preference.EditorFontSize = value;
    }

    partial void OnImprovedResourcePathChanged(string value)
    {
        _appSettings.Preference.ImprovedResourcePath = value;
    }

    partial void OnDefaultChineseVoiceNameChanged(string value)
    {
        _appSettings.Preference.DefaultChineseVoiceName = value;
    }

    partial void OnDefaultEnglishVoiceNameChanged(string value)
    {
        _appSettings.Preference.DefaultEnglishVoiceName = value;
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
