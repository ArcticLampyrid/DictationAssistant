using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using DictationAssistant.App.Services;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly EditableWordListSource _wordListSource;
    private readonly IDictationPlayer _dictationPlayer;
    private readonly ITextFileService _textFileService;

    public MainWindowViewModel(
        EditableWordListSource wordListSource,
        IDictationPlayer dictationPlayer,
        ITextFileService textFileService,
        ITtsEngine ttsEngine)
    {
        _wordListSource = wordListSource;
        _dictationPlayer = dictationPlayer;
        _textFileService = textFileService;

        TtsEngineName = ttsEngine.Name;
        _wordListSource.ReplaceLines([string.Empty]);

        _dictationPlayer.ProgressChanged += (_, progress) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (HighlightCurrentLine)
                {
                    CurrentLineIndex = progress.CurrentWordIndex;
                }

                ProgressText = progress.TotalWords <= 0
                    ? "0 / 0"
                    : $"{Math.Max(progress.CurrentWordIndex + 1, 0)} / {progress.TotalWords}";
                ProgressPercent = progress.Percent;
            });
        };

        _dictationPlayer.StateChanged += (_, state) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                CurrentState = state;
                OnPropertyChanged(nameof(IsAutoRunning));
                OnPropertyChanged(nameof(IsAutoPaused));
            });
        };

        SyncSettingsFromCore();
    }

    public IReadOnlyList<WordLineViewModel> Lines => _wordListSource.Lines;

    [ObservableProperty]
    private string _ttsEngineName = string.Empty;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _status = "Ready";

    [ObservableProperty]
    private int _currentLineIndex = -1;

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

    public bool IsAutoRunning => CurrentState == DictationState.AutoRunning;

    public bool IsAutoPaused => CurrentState == DictationState.AutoPaused;

    [RelayCommand]
    private void AddLine()
    {
        _wordListSource.Lines.Add(new WordLineViewModel { Text = string.Empty });
    }

    [RelayCommand]
    private void RemoveCurrentLine()
    {
        if (CurrentLineIndex < 0 || CurrentLineIndex >= _wordListSource.Lines.Count)
        {
            return;
        }

        _wordListSource.Lines.RemoveAt(CurrentLineIndex);
        if (_wordListSource.Lines.Count == 0)
        {
            _wordListSource.Lines.Add(new WordLineViewModel { Text = string.Empty });
        }
    }

    [RelayCommand]
    private async Task LoadFromFileAsync()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            Status = "Please set a file path first.";
            return;
        }

        var content = await _textFileService.ReadAllTextAsync(FilePath).ConfigureAwait(false);
        var lines = content.Replace("\r\n", "\n").Split('\n');
        _wordListSource.ReplaceLines(lines);
        Status = $"Loaded {lines.Length} lines from {FilePath}";
        ResetProgressUi();
    }

    [RelayCommand]
    private async Task SaveToFileAsync()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            Status = "Please set a file path first.";
            return;
        }

        await _textFileService.WriteAllTextAsync(FilePath, _wordListSource.ToText()).ConfigureAwait(false);
        Status = $"Saved to {FilePath}";
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
            OutputPath = FilePath + ".wav"
        };
        var result = await _dictationPlayer.SaveAudioAsync(request).ConfigureAwait(false);
        Status = result.Message;
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
        ProgressText = "0 / 0";
        ProgressPercent = 0;
    }
}
