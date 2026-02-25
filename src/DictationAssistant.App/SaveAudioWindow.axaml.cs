using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using DictationAssistant.App.ViewModels;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Models;

namespace DictationAssistant.App;

public partial class SaveAudioWindow : Window
{
    private SaveAudioWindowViewModel? ViewModel => DataContext as SaveAudioWindowViewModel;

    public SaveAudioWindow()
    {
        InitializeComponent();
        DataContext = new SaveAudioWindowViewModel();
    }

    public SaveAudioWindow(IDictationPlayer dictationPlayer) : this()
    {
        if (DataContext is SaveAudioWindowViewModel vm)
        {
            vm.SetDictationPlayer(dictationPlayer);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        Close(false);
    }

    private async void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (ViewModel is { } vm)
        {
            var success = await vm.ExportAsync(CancellationToken.None);
            Close(success);
            return;
        }

        Close(false);
    }

    private async void SetTargetPath_Click(object? sender, RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (ViewModel is null)
        {
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "选择目标文件",
            SuggestedFileName = Path.GetFileName(ViewModel.TargetPath),
            DefaultExtension = ViewModel.OutputFormat,
            FileTypeChoices =
            [
                new FilePickerFileType($"{ViewModel.OutputFormat} 文件")
                {
                    Patterns = [$"*.{ViewModel.OutputFormat}"]
                }
            ]
        });

        if (file is null)
        {
            return;
        }

        ViewModel.TargetPath = file.TryGetLocalPath() ?? file.Name;
    }
}
