using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using DictationAssistant.App.ViewModels;

namespace DictationAssistant.App;

public partial class PreferenceWindow : Window
{
    private PreferenceWindowViewModel? ViewModel => DataContext as PreferenceWindowViewModel;

    public PreferenceWindow()
    {
        InitializeComponent();
        DataContext = new PreferenceWindowViewModel();
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
        Close(true);
    }

    private async void SetPathForImprovedResourceButton_Click(object? sender, RoutedEventArgs e)
    {
        _ = sender;
        _ = e;
        if (ViewModel is null)
        {
            return;
        }

        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "选择音源增强目录",
            AllowMultiple = false
        });

        var folder = folders.FirstOrDefault();
        if (folder is null)
        {
            return;
        }

        ViewModel.ImprovedResourcePath = folder.TryGetLocalPath() ?? folder.Name;
    }
}
