using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DictationAssistant.App.Services;
using DictationAssistant.App.Services.Audio;
using DictationAssistant.App.Services.Settings;
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
            var settingsStore = new AppSettingsStore();
            var appSettings = settingsStore.Load();
            var wordListSource = new EditorDocumentWordListSource();
            var audioPlayer = new SdlPcmPlayer();
            IPcmTtsEngine ttsEngine = TtsEngineFactory.CreateDefaultPcmEngine();
            IDictationPlayer player = new DictationPlayer(ttsEngine, wordListSource, audioPlayer);

            desktop.MainWindow = new MainWindow(appSettings)
            {
                DataContext = new MainWindowViewModel(wordListSource, player, new LocalTextFileService(), ttsEngine, appSettings)
            };

            desktop.MainWindow.Closing += (_, _) => settingsStore.Save(appSettings);

            desktop.Exit += (_, _) =>
            {
                settingsStore.Save(appSettings);
                audioPlayer.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
