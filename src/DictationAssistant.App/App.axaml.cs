using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DictationAssistant.App.Services;
using DictationAssistant.App.Services.Tts;
using DictationAssistant.App.ViewModels;
using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Services;

namespace DictationAssistant.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var wordListSource = new EditableWordListSource();
            ITtsEngine ttsEngine = TtsEngineFactory.CreateDefault();
            IDictationPlayer player = new DictationPlayer(ttsEngine, wordListSource);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(wordListSource, player, new LocalTextFileService(), ttsEngine)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
