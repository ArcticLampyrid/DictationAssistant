using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using DictationAssistant.App.ViewModels;

namespace DictationAssistant.App;

public partial class SaveAudioWindow : Window
{
    private SaveAudioWindowViewModel? ViewModel => DataContext as SaveAudioWindowViewModel;

    public SaveAudioWindow()
    {
        InitializeComponent();
        DataContext = new SaveAudioWindowViewModel();
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

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (ViewModel is not null)
        {
            ViewModel.Status = "v4 暂未实现";
        }

        Close(true);
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
