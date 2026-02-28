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
    private readonly AppSettings _appSettings;
    private readonly AppSettingsStore _settingsStore;
    private readonly VoiceAggregator _aggregator;
    private IVoice _currentVoice;
    private DispatcherTimer? _countdownTimer;
    private CancellationTokenSource? _saveCts;

    /// <summary>
    /// Raised when a user-facing alert message should be shown.
    /// The View subscribes to this and shows a MessageBox.
    /// </summary>
    public event Action<string>? AlertRequested;

    public IDictationPlayer DictationPlayer => _dictationPlayer;

    public MainWindowViewModel(
        EditorDocumentWordListSource wordListSource,
        IAudioPlayer audioPlayer,
        VoiceAggregator aggregator,
        IVoice initialVoice,
        AppSettings appSettings,
        AppSettingsStore settingsStore)
    {
        _wordListSource = wordListSource;
        _appSettings = appSettings;
        _settingsStore = settingsStore;
        _aggregator = aggregator;
        _currentVoice = initialVoice;
        _appSettings.EnsureDefaults();

        var dictationSettings = new DictationSettings
        {
            IntervalExpression = appSettings.Dictation.IntervalExpression,
            HighlightCurrentLine = appSettings.Dictation.HighlightCurrentLine,
            AutoScrollToCurrentLine = appSettings.Dictation.AutoScrollToCurrentLine,
            DefaultChineseVoiceId = appSettings.Preference.DefaultChineseVoiceId,
            DefaultEnglishVoiceId = appSettings.Preference.DefaultEnglishVoiceId
        };

        _dictationPlayer = new DictationPlayer(_currentVoice, wordListSource, audioPlayer, dictationSettings);
        _dictationPlayer.ProgressChanged += (_, progress) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                CurrentLineIndex = progress.CurrentWordIndex;
                CurrentRepeat = progress.CurrentRepeat;

                ProgressText = progress.TotalWords <= 0
                    ? "0 / 0"
                    : $"{Math.Max(progress.CurrentWordIndex + 1, 0)} / {progress.TotalWords}";
                OnPropertyChanged(nameof(IsAutoRunning));
                OnPropertyChanged(nameof(IsAutoPaused));
                OnPropertyChanged(nameof(PauseOrResumeAutoText));
                OnPropertyChanged(nameof(SpeakStateText));
                UpdateCountdownTimer();
            });
        };

        LoadSettings();
        _ = LoadVoiceOptionsAsync();
        OnPropertyChanged(nameof(SpeakStateText));
    }

    public EditorDocumentWordListSource WordListSource => _wordListSource;

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
    private string _defaultChineseVoiceId = string.Empty;

    [ObservableProperty]
    private string _defaultEnglishVoiceId = string.Empty;

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

            var progress = _dictationPlayer.Progress;

            if (_dictationPlayer.IsSpeaking)
            {
                var speakingIndex = Math.Max(progress.CurrentWordIndex + 1, 1);
                return $"正在播报第{speakingIndex}个";
            }

            if (_dictationPlayer.AutoMode && progress.NextWordIndex.HasValue)
            {
                var nextIndex = progress.NextWordIndex.Value + 1;
                if (progress.NextSpeakTime.HasValue)
                {
                    var remaining = progress.NextSpeakTime.Value - DateTimeOffset.Now;
                    if (remaining.TotalSeconds > 0)
                    {
                        return $"即将播报第{nextIndex}个 ({remaining.TotalSeconds:F1}秒后)";
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

    private void UpdateCountdownTimer()
    {
        var progress = _dictationPlayer.Progress;
        if (progress.NextSpeakTime.HasValue && _dictationPlayer.AutoMode && !_dictationPlayer.IsPaused)
        {
            if (_countdownTimer is null)
            {
                _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                _countdownTimer.Tick += (_, _) =>
                {
                    OnPropertyChanged(nameof(SpeakStateText));

                    var p = _dictationPlayer.Progress;
                    if (!p.NextSpeakTime.HasValue || !_dictationPlayer.AutoMode || _dictationPlayer.IsPaused)
                    {
                        StopCountdownTimer();
                    }
                };
            }
            _countdownTimer.Start();
        }
        else
        {
            StopCountdownTimer();
        }
    }

    private void StopCountdownTimer()
    {
        _countdownTimer?.Stop();
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

    [RelayCommand]
    private void SpeakPrevious()
    {
        if (_dictationPlayer.Progress.CurrentWordIndex <= 0)
        {
            AlertRequested?.Invoke("已经是第1个了！");
            return;
        }
        _dictationPlayer.SpeakPrevious();
    }

    [RelayCommand]
    private void SpeakAgain()
    {
        if (_dictationPlayer.Progress.CurrentWordIndex < 0 || _dictationPlayer.Progress.CurrentWordIndex >= _wordListSource.Count)
        {
            AlertRequested?.Invoke("还没报过或已移除报过的词语！");
            return;
        }
        _dictationPlayer.SpeakAgain();
    }

    [RelayCommand]
    private void SpeakNext()
    {
        if (_wordListSource.Count == 0)
        {
            AlertRequested?.Invoke("请先添加词语！");
            return;
        }
        if (_dictationPlayer.Progress.CurrentWordIndex >= _wordListSource.Count - 1)
        {
            AlertRequested?.Invoke("已经播完了。");
            return;
        }
        _dictationPlayer.SpeakNext();
    }

    [RelayCommand]
    private void StartAuto()
    {
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

    public void SpeakLine(int index)
    {
        _dictationPlayer.SpeakAt(index);
    }

    public void StartAutoFromLine(int index)
    {
        _dictationPlayer.StartAuto(index);
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

                var targetVoiceId = _appSettings.Dictation.LastSelectedVoiceId;
                if (string.IsNullOrWhiteSpace(targetVoiceId))
                {
                    targetVoiceId = DefaultChineseVoiceId ?? DefaultEnglishVoiceId;
                }

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
            _appSettings.Dictation.LastSelectedVoiceId = value.Info.Id;
            ScheduleSave();

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
            AlertRequested?.Invoke("未能找到中文引擎");
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
            AlertRequested?.Invoke("未能找到英文引擎");
        }
    }

    public PreferenceSettings CreatePreferenceSnapshot()
    {
        return new PreferenceSettings
        {
            EditorFontFamily = EditorFontFamily,
            EditorFontSize = EditorFontSize,
            ImprovedResourcePath = ImprovedResourcePath,
            DefaultChineseVoiceId = DefaultChineseVoiceId,
            DefaultEnglishVoiceId = DefaultEnglishVoiceId
        };
    }

    public void ApplyPreferenceSettings(PreferenceSettings settings)
    {
        EditorFontFamily = settings.EditorFontFamily;
        EditorFontSize = settings.EditorFontSize;
        ImprovedResourcePath = settings.ImprovedResourcePath;
        DefaultChineseVoiceId = settings.DefaultChineseVoiceId;
        DefaultEnglishVoiceId = settings.DefaultEnglishVoiceId;
    }

    public void SaveSettings()
    {
        _settingsStore.Save(_appSettings);
    }

    private void LoadSettings()
    {
        IntervalExpression = _appSettings.Dictation.IntervalExpression;
        TimesPerWord = _appSettings.Dictation.TimesPerWord;
        HighlightCurrentLine = _appSettings.Dictation.HighlightCurrentLine;
        AutoScrollCurrentLine = _appSettings.Dictation.AutoScrollToCurrentLine;
        Volume = _appSettings.Dictation.Volume;
        Rate = _appSettings.Dictation.Rate;

        EditorFontFamily = _appSettings.Preference.EditorFontFamily;
        EditorFontSize = _appSettings.Preference.EditorFontSize;
        ImprovedResourcePath = _appSettings.Preference.ImprovedResourcePath;
        DefaultChineseVoiceId = _appSettings.Preference.DefaultChineseVoiceId;
        DefaultEnglishVoiceId = _appSettings.Preference.DefaultEnglishVoiceId;
    }

    partial void OnWordListVisibleChanged(bool value)
    {
        OnPropertyChanged(nameof(ShowOrHideWordListText));
    }

    private void ScheduleSave()
    {
        _saveCts?.Cancel();
        _saveCts = new CancellationTokenSource();
        var ct = _saveCts.Token;
        Task.Delay(500, ct).ContinueWith(_ =>
        {
            if (!ct.IsCancellationRequested)
                _settingsStore.Save(_appSettings);
        }, TaskScheduler.Default);
    }

    partial void OnIntervalExpressionChanged(string value)
    {
        _appSettings.Dictation.IntervalExpression = value;
        ValidateAndApplyWaitingTime(value);
        ScheduleSave();
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
        _dictationPlayer.TimesPerWord = _appSettings.Dictation.TimesPerWord;
        ScheduleSave();
    }

    partial void OnHighlightCurrentLineChanged(bool value)
    {
        _appSettings.Dictation.HighlightCurrentLine = value;
        ScheduleSave();
    }

    partial void OnAutoScrollCurrentLineChanged(bool value)
    {
        _appSettings.Dictation.AutoScrollToCurrentLine = value;
        ScheduleSave();
    }

    partial void OnVolumeChanged(int value)
    {
        _appSettings.Dictation.Volume = value;
        _dictationPlayer.Volume = _appSettings.Dictation.Volume;
        ScheduleSave();
    }

    partial void OnRateChanged(int value)
    {
        _appSettings.Dictation.Rate = value;
        _dictationPlayer.Rate = _appSettings.Dictation.Rate;
        ScheduleSave();
    }

    partial void OnEditorFontFamilyChanged(string value)
    {
        _appSettings.Preference.EditorFontFamily = value;
        ScheduleSave();
    }

    partial void OnEditorFontSizeChanged(double value)
    {
        _appSettings.Preference.EditorFontSize = value;
        ScheduleSave();
    }

    partial void OnImprovedResourcePathChanged(string value)
    {
        _appSettings.Preference.ImprovedResourcePath = value;
        ScheduleSave();
    }

    partial void OnDefaultChineseVoiceIdChanged(string value)
    {
        _appSettings.Preference.DefaultChineseVoiceId = value;
        ScheduleSave();
    }

    partial void OnDefaultEnglishVoiceIdChanged(string value)
    {
        _appSettings.Preference.DefaultEnglishVoiceId = value;
        ScheduleSave();
    }
}
