using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaEdit;
using DictationAssistant.App.Services.Settings;
using DictationAssistant.App.ViewModels;

namespace DictationAssistant.App;

public partial class MainWindow : Window
{
    private readonly AppSettings _appSettings;
    private TextEditor? _wordlistEditor;
    private IStorageFile? _currentFile;
    private bool _isApplyingViewModelPosition;

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
            if (DataContext is MainWindowViewModel vm)
            {
                HookEditor(vm);
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

        Closing += (_, _) =>
        {
            _appSettings.MainWindow.Width = Width;
            _appSettings.MainWindow.Height = Height;
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
        _wordlistEditor.TextArea.Caret.PositionChanged += (_, _) => SyncCurrentLineFromCaret(vm);
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
            if (vm.HighlightCurrentLine)
            {
                _wordlistEditor.Select(0, 0);
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
            }

            if (vm.HighlightCurrentLine)
            {
                _wordlistEditor.Select(line.Offset, Math.Max(line.Length, 0));
            }
            else
            {
                _wordlistEditor.Select(0, 0);
            }
        }
        finally
        {
            _isApplyingViewModelPosition = false;
        }
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
            vm.Status = $"已加载：{path}";
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
            vm.Status = $"已保存：{path}";
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
            vm.Status = "已新建空白词语列表";
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

        var dialog = new PreferenceWindow(vm.CreatePreferenceSnapshot());
        var result = await dialog.ShowDialog<bool?>(this);
        if (result == true)
        {
            vm.ApplyPreferenceSettings(dialog.ResultSettings);
            vm.Status = "偏好设置已保存";
        }
    }

    private async void SaveAudio_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        var dialog = new SaveAudioWindow();
        var result = await dialog.ShowDialog<bool?>(this);
        if (GetViewModel() is { } vm)
        {
            vm.Status = result == true ? "v4 暂未实现" : "已取消保存音频";
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

    private void SelectAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        _wordlistEditor?.SelectAll();
    }

    private void Count_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var count = _wordlistEditor.Document.LineCount;
        vm.Status = $"共 {count} 行";
    }

    private async void SpeakSelection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var index = Math.Max(_wordlistEditor.TextArea.Caret.Line - 1, 0);
        await vm.SpeakLineAsync(index);
    }

    private void ViewSelectionInBingDictionary_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
                vm.Status = "无法打开浏览器";
            }
        }
    }

    private async void StartFromSelection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (_wordlistEditor is null || GetViewModel() is not { } vm)
        {
            return;
        }

        var index = Math.Max(_wordlistEditor.TextArea.Caret.Line - 1, 0);
        await vm.StartAutoFromLineAsync(index);
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

        await vm.StopCommand.ExecuteAsync(null);
        vm.CurrentLineIndex = 0;
        vm.CurrentRepeat = 0;
        vm.ProgressText = "0 / 0";
        vm.ProgressPercent = 0;
        vm.Status = "已归零";
    }
}
