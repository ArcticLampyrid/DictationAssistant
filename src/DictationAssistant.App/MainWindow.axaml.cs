using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using DictationAssistant.App.Settings;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using DictationAssistant.App.ViewModels;
using XmlReader = System.Xml.XmlReader;

namespace DictationAssistant.App;

public partial class MainWindow : Window
{
    private const string NormalWindowStateName = "Normal";
    private readonly AppSettings _appSettings;
    private TextEditor? _wordlistEditor;
    private IStorageFile? _currentFile;
    private bool _isApplyingViewModelPosition;
    private bool _windowPlacementRestored;
    private HighlightedLineBackgroundRenderer? _highlightedLineRenderer;

    public MainWindow()
        : this(new AppSettings())
    {
    }

    public MainWindow(AppSettings appSettings)
    {
        _appSettings = appSettings;
        InitializeComponent();

        Opened += (_, _) =>
        {
            RestoreWindowPlacementIfNeeded();

            if (DataContext is MainWindowViewModel vm)
            {
                HookEditor(vm);
                vm.AlertRequested += message =>
                {
                    Dispatcher.UIThread.Post(async () =>
                    {
                        var box = MessageBoxManager.GetMessageBoxStandard("自动默写", message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
                        await box.ShowWindowDialogAsync(this);
                    });
                };
                vm.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(MainWindowViewModel.CurrentLineIndex) ||
                        args.PropertyName == nameof(MainWindowViewModel.HighlightCurrentLine) ||
                        args.PropertyName == nameof(MainWindowViewModel.AutoScrollCurrentLine))
                    {
                        Dispatcher.UIThread.Post(() => MoveEditorToCurrentLine(vm));
                    }
                };
            }
        };

        AddHandler(DragDrop.DropEvent, OnDrop);

        Closing += (_, _) =>
        {
            PersistWindowPlacement();
            if (GetViewModel() is { } vm)
            {
                _appSettings.MainWindow.WordListVisible = vm.WordListVisible;
            }
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _wordlistEditor = this.FindControl<TextEditor>("WordlistEditor");
    }

    private void HookEditor(MainWindowViewModel vm)
    {
        if (_wordlistEditor is null)
        {
            return;
        }

        vm.WordListSource.AttachDocument(_wordlistEditor.Document);

        var highlightingAssembly = Assembly.GetExecutingAssembly();
        using var highlightingStream = highlightingAssembly.GetManifestResourceStream("DictationAssistant.App.Resources.WordlistHighlighting.xshd");
        if (highlightingStream != null)
        {
            using var reader = XmlReader.Create(highlightingStream);
            _wordlistEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }

        _highlightedLineRenderer = new HighlightedLineBackgroundRenderer(_wordlistEditor.TextArea.TextView)
        {
            Background = new SolidColorBrush(Colors.LightGreen)
        };
        _wordlistEditor.TextArea.TextView.BackgroundRenderers.Add(_highlightedLineRenderer);
    }

    private void SyncCurrentLineFromCaret(MainWindowViewModel vm)
    {
        if (_wordlistEditor is null || _isApplyingViewModelPosition)
        {
            return;
        }

        var line = _wordlistEditor.TextArea.Caret.Line;
        vm.CurrentLineIndex = Math.Max(line - 1, 0);
    }

    private void MoveEditorToCurrentLine(MainWindowViewModel vm)
    {
        if (_wordlistEditor is null || _wordlistEditor.Document.LineCount <= 0)
        {
            return;
        }

        var index = vm.CurrentLineIndex;
        if (index < 0)
        {
            if (_highlightedLineRenderer is { } renderer)
            {
                renderer.LineNumber = 0;
            }

            return;
        }

        var lineNumber = Math.Clamp(index + 1, 1, _wordlistEditor.Document.LineCount);
        var line = _wordlistEditor.Document.GetLineByNumber(lineNumber);

        _isApplyingViewModelPosition = true;
        try
        {
            _wordlistEditor.TextArea.Caret.Offset = line.Offset;
            if (vm.AutoScrollCurrentLine)
            {
                _wordlistEditor.ScrollToLine(lineNumber);
                _wordlistEditor.TextArea.Caret.BringCaretToView();
            }

            if (_highlightedLineRenderer is { } renderer)
            {
                renderer.LineNumber = vm.HighlightCurrentLine ? lineNumber : 0;
            }
        }
        finally
        {
            _isApplyingViewModelPosition = false;
        }
    }

    private void RestoreWindowPlacementIfNeeded()
    {
        if (_windowPlacementRestored)
        {
            return;
        }

        _windowPlacementRestored = true;

        var settings = _appSettings.MainWindow;
        var minAllowedWidth = Math.Max(MinWidth, 1d);
        var minAllowedHeight = Math.Max(MinHeight, 1d);

        var width = NormalizeDimension(settings.Width, minAllowedWidth);
        var height = NormalizeDimension(settings.Height, minAllowedHeight);

        Width = width;
        Height = height;

        if (settings.X is { } x && settings.Y is { } y && IsValidPosition(x) && IsValidPosition(y) && IsRectOnAnyScreen(x, y, width, height))
        {
            Position = new PixelPoint((int)Math.Round(x), (int)Math.Round(y));
        }

        if (TryParseWindowState(settings.WindowState, out var windowState))
        {
            WindowState = windowState;
        }

        _appSettings.MainWindow.Width = Width;
        _appSettings.MainWindow.Height = Height;
        _appSettings.MainWindow.WindowState = WindowState.ToString();
    }

    private void PersistWindowPlacement()
    {
        var settings = _appSettings.MainWindow;
        settings.WindowState = WindowState.ToString();

        if (WindowState == WindowState.Normal)
        {
            settings.Width = Width;
            settings.Height = Height;
            settings.X = Position.X;
            settings.Y = Position.Y;
            return;
        }

        if (string.IsNullOrWhiteSpace(settings.WindowState))
        {
            settings.WindowState = NormalWindowStateName;
        }
    }

    private bool IsRectOnAnyScreen(double x, double y, double width, double height)
    {
        var screens = Screens.All;
        if (screens.Count == 0)
        {
            return true;
        }

        var rect = new PixelRect(
            (int)Math.Round(x),
            (int)Math.Round(y),
            Math.Max((int)Math.Round(width), 1),
            Math.Max((int)Math.Round(height), 1));

        foreach (var screen in screens)
        {
            if (RectsOverlap(rect, screen.WorkingArea))
            {
                return true;
            }
        }

        return false;
    }

    private static bool RectsOverlap(PixelRect a, PixelRect b)
    {
        return a.X < b.X + b.Width
            && a.X + a.Width > b.X
            && a.Y < b.Y + b.Height
            && a.Y + a.Height > b.Y;
    }

    private static bool TryParseWindowState(string? value, out WindowState windowState)
    {
        if (Enum.TryParse(value, ignoreCase: true, out windowState))
        {
            return true;
        }

        windowState = WindowState.Normal;
        return false;
    }

    private static double NormalizeDimension(double value, double minimum)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || value < minimum)
        {
            return minimum;
        }

        return value;
    }

    private static bool IsValidPosition(double value)
    {
        return !double.IsNaN(value) && !double.IsInfinity(value) && value >= 0;
    }

    private MainWindowViewModel? GetViewModel() => DataContext as MainWindowViewModel;

    private async void OpenFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null)
        {
            return;
        }

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "打开词语列表",
            AllowMultiple = false
        });

        var file = files.FirstOrDefault();
        if (file is null)
        {
            return;
        }

        await using var stream = await file.OpenReadAsync();
        using var reader = new StreamReader(stream);
        _wordlistEditor.Text = await reader.ReadToEndAsync();
        _currentFile = file;

        if (GetViewModel() is { } vm)
        {
            var path = file.TryGetLocalPath() ?? file.Name;
            vm.FilePath = path;
            vm.CurrentLineIndex = 0;
        }
    }

    private async void SaveFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null)
        {
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存词语列表",
            SuggestedFileName = _currentFile?.Name ?? "wordlist.txt",
            DefaultExtension = "txt"
        });

        if (file is null)
        {
            return;
        }

        await using var stream = await file.OpenWriteAsync();
        stream.SetLength(0);
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(_wordlistEditor.Text);
        await writer.FlushAsync();

        _currentFile = file;
        if (GetViewModel() is { } vm)
        {
            var path = file.TryGetLocalPath() ?? file.Name;
            vm.FilePath = path;
        }
    }

    private void NewWordList_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null)
        {
            return;
        }

        _wordlistEditor.Text = string.Empty;
        _currentFile = null;

        if (GetViewModel() is { } vm)
        {
            vm.FilePath = string.Empty;
                vm.CurrentLineIndex = 0;
        }
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        Close();
    }

    private async void Preference_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (GetViewModel() is not { } vm)
        {
            return;
        }

        var voiceNames = vm.VoiceOptions.Select(v => v.Info.DisplayName).ToList();
        var dialog = new PreferenceWindow(vm.CreatePreferenceSnapshot(), voiceNames);
        var result = await dialog.ShowDialog<bool?>(this);
        if (result == true)
        {
            vm.ApplyPreferenceSettings(dialog.ResultSettings);
            }
    }

    private async void SaveAudio_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        var vm = GetViewModel();
        if (vm is null)
        {
            return;
        }

        var dialog = new SaveAudioWindow(vm.DictationPlayer);
        var result = await dialog.ShowDialog<bool?>(this);
        if (GetViewModel() is { } currentVm)
        {
        }
    }

    private async void About_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        var dialog = new AboutWindow();
        await dialog.ShowDialog(this);
    }

    private void Cut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.Cut();
    }

    private void Copy_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.Copy();
    }

    private void Paste_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.Paste();
    }

    private void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null)
        {
            return;
        }

        if (_wordlistEditor.SelectionLength > 0)
        {
            _wordlistEditor.SelectedText = string.Empty;
        }
    }

    private void Undo_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.Undo();
    }

    private void Redo_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.Redo();
    }

    private void SelectAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.SelectAll();
    }

    private async void Count_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var count = _wordlistEditor.Document.LineCount;
        var box = MessageBoxManager.GetMessageBoxStandard("自动默写", $"词语数量：{count}", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
        await box.ShowWindowDialogAsync(this);
    }

    private void SpeakSelection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var index = Math.Max(_wordlistEditor.TextArea.Caret.Line - 1, 0);
        vm.SpeakLine(index);
    }

    private async void ViewSelectionInBingDictionary_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null)
        {
            return;
        }

        var text = string.IsNullOrWhiteSpace(_wordlistEditor.SelectedText)
            ? ReadCaretLineText(_wordlistEditor)
            : _wordlistEditor.SelectedText;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var query = Uri.EscapeDataString(text.Trim());
        var url = $"https://cn.bing.com/dict/search?q={query}";
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
            if (GetViewModel() is { } vm)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("自动默写", "无法打开浏览器", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                    await box.ShowWindowDialogAsync(this);
            }
        }
    }

    private void StartFromSelection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var index = Math.Max(_wordlistEditor.TextArea.Caret.Line - 1, 0);
        vm.StartAutoFromLine(index);
    }

    private static string ReadCaretLineText(TextEditor editor)
    {
        var lineNumber = Math.Max(editor.TextArea.Caret.Line, 1);
        var line = editor.Document.GetLineByNumber(lineNumber);
        return editor.Document.GetText(line.Offset, line.Length);
    }

    private async void ResetRecord_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (GetViewModel() is not { } vm)
        {
            return;
        }

        if (vm.DictationPlayer.AutoMode)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("自动默写", "请先停止自动播报！", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
            await box.ShowWindowDialogAsync(this);
            return;
        }

        vm.DictationPlayer.ResetProgress();
    }
    private async void OnDrop(object? sender, DragEventArgs e)
    {
        if (_wordlistEditor is null)
        {
            return;
        }

#pragma warning disable CS0618 // Avalonia 11: DataTransfer does not support GetFiles yet
        var files = e.Data.GetFiles()?.ToList();
#pragma warning restore CS0618
        if (files is null || files.Count == 0)
        {
            return;
        }

        var file = files[0];
        var localPath = file.TryGetLocalPath();
        if (localPath is null || !File.Exists(localPath))
        {
            return;
        }

        try
        {
            var text = await File.ReadAllTextAsync(localPath);
            _wordlistEditor.Text = text;

            if (GetViewModel() is { } vm)
            {
                vm.FilePath = localPath;
                vm.CurrentLineIndex = 0;
            }
        }
        catch (Exception ex)
        {
            if (GetViewModel() is { } vm)
            {
                var errBox2 = MessageBoxManager.GetMessageBoxStandard("自动默写", $"无法打开文件：{ex.Message}", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                    await errBox2.ShowWindowDialogAsync(this);
            }
        }
    }

    public async Task LoadFileByPathAsync(string filePath)
    {
        if (_wordlistEditor is null)
        {
            return;
        }

        try
        {
            var text = await File.ReadAllTextAsync(filePath);
            _wordlistEditor.Text = text;

            if (GetViewModel() is { } vm)
            {
                vm.FilePath = filePath;
            }
        }
        catch (Exception ex)
        {
            var errBox = MessageBoxManager.GetMessageBoxStandard("自动默写", $"无法打开文件：{ex.Message}", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await errBox.ShowWindowDialogAsync(this);
        }
    }

}
