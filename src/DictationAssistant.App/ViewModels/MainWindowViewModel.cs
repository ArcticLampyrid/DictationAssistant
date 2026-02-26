using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using System.Diagnostics;
using DictationAssistant.App.Services;
using DictationAssistant.App.Settings;
using DictationAssistant.App.Voice;
using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Models;

namespace DictationAssistant.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly EditorDocumentWordListSource _wordListSource;
    private readonly IDictationPlayer _dictationPlayer;
    private readonly ITextFileService _textFileService;
    private readonly AppSettings _appSettings;
    private readonly VoiceAggregator _aggregator;
    private IVoice _currentVoice;

    public IDictationPlayer DictationPlayer => _dictationPlayer;

    public MainWindowViewModel(
        EditorDocumentWordListSource wordListSource,
        IAudioPlayer audioPlayer,
        ITextFileService textFileService,
        VoiceAggregator aggregator,
        IVoice initialVoice,
        AppSettings appSettings)
    {
        _wordListSource = wordListSource;
        _textFileService = textFileService;
        _appSettings = appSettings;
        _aggregator = aggregator;
        _currentVoice = initialVoice;
        _appSettings.EnsureDefaults();

        var dictationSettings = new DictationSettings
        {
            IntervalExpression = appSettings.Dictation.IntervalExpression,
            HighlightCurrentLine = appSettings.Dictation.HighlightCurrentLine,
            AutoScrollToCurrentLine = appSettings.Dictation.AutoScrollToCurrentLine,
            DefaultChineseVoiceName = appSettings.Preference.DefaultChineseVoiceName,
            DefaultEnglishVoiceName = appSettings.Preference.DefaultEnglishVoiceName
        };

        _dictationPlayer = new DictationPlayer(_currentVoice, wordListSource, audioPlayer, dictationSettings);
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
                OnPropertyChanged(nameof(IsAutoRunning));
                OnPropertyChanged(nameof(IsAutoPaused));
                OnPropertyChanged(nameof(PauseOrResumeAutoText));
                OnPropertyChanged(nameof(SpeakStateText));
            });
        };

        LoadSettings();
        ApplySettingsToCore();
        _ = LoadVoiceOptionsAsync();
        OnPropertyChanged(nameof(SpeakStateText));
    }

    public EditorDocumentWordListSource WordListSource => _wordListSource;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _status = string.Empty;

    [ObservableProperty]
    private int _currentLineIndex = -1;

    [ObservableProperty]
    private int _currentRepeat;

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
    private ObservableCollection<IVoiceFactory> _voiceOptions = [];

    [ObservableProperty]
    private IVoiceFactory? _selectedVoiceFactory;

    public bool IsAutoRunning => _dictationPlayer.AutoMode && !_dictationPlayer.IsPaused;

    public bool IsAutoPaused => _dictationPlayer.IsPaused;

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

            if (_dictationPlayer.IsSpeaking)
            {
                var speakingIndex = Math.Max(CurrentLineIndex + 1, 1);
                return $"正在播报第{speakingIndex}个";
            }

            var progress = _dictationPlayer.Progress;
            if (_dictationPlayer.AutoMode && progress.NextWordIndex.HasValue)
            {
                var nextIndex = progress.NextWordIndex.Value + 1;
                if (progress.NextSpeakTime.HasValue)
                {
                    var remaining = progress.NextSpeakTime.Value - DateTimeOffset.Now;
                    if (remaining.TotalSeconds > 0)
                    {
                        return $"即将播报第{nextIndex}个 ({Math.Ceiling(remaining.TotalSeconds)}秒后)";
                    }
                }
                return $"即将播报第{nextIndex}个";
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
    private void SpeakPrevious()
    {
        ApplySettingsToCore();
        _dictationPlayer.SpeakPrevious();
    }

    [RelayCommand]
    private void SpeakAgain()
    {
        ApplySettingsToCore();
        _dictationPlayer.SpeakAgain();
    }

    [RelayCommand]
    private void SpeakNext()
    {
        ApplySettingsToCore();
        _dictationPlayer.SpeakNext();
    }

    [RelayCommand]
    private void StartAuto()
    {
        ApplySettingsToCore();
        _dictationPlayer.StartAuto(0);
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
    private void Stop()
    {
        _dictationPlayer.Stop();
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

    public void SpeakLine(int index)
    {
        CurrentLineIndex = index;
        ApplySettingsToCore();
        _dictationPlayer.SpeakAt(index);
    }

    public void StartAutoFromLine(int index)
    {
        CurrentLineIndex = index;
        ApplySettingsToCore();
        _dictationPlayer.StartAuto(index);
    }

    private void ApplySettingsToCore()
    {
        _dictationPlayer.TimesPerWord = TimesPerWord;
        _dictationPlayer.Volume = Volume;
        _dictationPlayer.Rate = Rate;
    }

    private async Task LoadVoiceOptionsAsync()
    {
        try
        {
            var factories = await _aggregator.GetAllFactoriesAsync(CancellationToken.None).ConfigureAwait(false);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                VoiceOptions.Clear();
                foreach (var factory in factories)
                {
                    VoiceOptions.Add(factory);
                }

                var targetVoiceId = DefaultChineseVoiceName ?? DefaultEnglishVoiceName;
                if (!string.IsNullOrWhiteSpace(targetVoiceId))
                {
                    SelectedVoiceFactory = VoiceOptions.FirstOrDefault(f => f.Info.Id == targetVoiceId || f.Info.DisplayName == targetVoiceId);
                }

                if (SelectedVoiceFactory is null && VoiceOptions.Count > 0)
                {
                    SelectedVoiceFactory = VoiceOptions[0];
                }
            });
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Failed to load voice factories: {ex}");
            await Dispatcher.UIThread.InvokeAsync(() => VoiceOptions.Clear());
        }
    }

    partial void OnSelectedVoiceFactoryChanged(IVoiceFactory? value)
    {
        if (value is not null)
        {
            DefaultChineseVoiceName = value.Info.Id;
            DefaultEnglishVoiceName = value.Info.Id;

            var newVoice = value.Create();

            if (!string.IsNullOrWhiteSpace(ImprovedResourcePath))
            {
                newVoice = new ImprovedVoice(newVoice, ImprovedResourcePath);
            }

            _currentVoice = newVoice;

            if (_dictationPlayer is DictationPlayer player)
            {
                player.Voice = _currentVoice;
            }
        }
    }

    [RelayCommand]
    private void SwitchToChineseVoice()
    {
        var factory = VoiceOptions.FirstOrDefault(f => f.Info.LocaleOrLanguage?.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == true);
        if (factory is not null)
        {
            SelectedVoiceFactory = factory;
        }
        else
        {
            Status = "未能找到中文引擎";
        }
    }

    [RelayCommand]
    private void SwitchToEnglishVoice()
    {
        var factory = VoiceOptions.FirstOrDefault(f => f.Info.LocaleOrLanguage?.StartsWith("en", StringComparison.OrdinalIgnoreCase) == true);
        if (factory is not null)
        {
            SelectedVoiceFactory = factory;
        }
        else
        {
            Status = "未能找到英文引擎";
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
        AutoScrollCurrentLine = _appSettings.Dictation.AutoScrollToCurrentLine;
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
        var result = WaitingTimeParser.Validate(expression);
        if (result.IsValid)
        {
            IntervalValidationHint = string.Empty;
            if (_dictationPlayer is DictationPlayer player)
            {
                player.WaitingTimeCalculator = result.Calculator;
            }
        }
        else
        {
            IntervalValidationHint = result.ErrorMessage;
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
        _appSettings.Dictation.AutoScrollToCurrentLine = value;
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
}
