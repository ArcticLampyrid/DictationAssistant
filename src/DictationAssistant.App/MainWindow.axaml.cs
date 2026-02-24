using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DictationAssistant.App.ViewModels;

namespace DictationAssistant.App;

public partial class MainWindow : Window
{
    private DataGrid? _wordGrid;

    public MainWindow()
    {
        InitializeComponent();

        Opened += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(MainWindowViewModel.CurrentLineIndex) && vm.AutoScrollCurrentLine)
                    {
                        Dispatcher.UIThread.Post(() => ScrollToCurrentLine(vm));
                    }
                };
            }
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _wordGrid = this.FindControl<DataGrid>("WordGrid");
    }

    private void ScrollToCurrentLine(MainWindowViewModel vm)
    {
        if (_wordGrid is null)
        {
            return;
        }

        if (vm.CurrentLineIndex < 0 || vm.CurrentLineIndex >= vm.Lines.Count)
        {
            return;
        }

        var item = vm.Lines[vm.CurrentLineIndex];
        _wordGrid.ScrollIntoView(item, null);
    }
}
