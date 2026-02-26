using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DictationAssistant.App.Services;
using DictationAssistant.App.Services.Audio;
using DictationAssistant.App.Services.Settings;
using DictationAssistant.App.Services.Voice;
using DictationAssistant.App.ViewModels;
using DictationAssistant.App.Abstractions;

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

            var providers = new List<IVoiceFactoryProvider>();
            if (OperatingSystem.IsWindows())
            {
                providers.Add(new SapiVoiceFactoryProvider());
            }

            if (OperatingSystem.IsMacOS() && ProcessRunner.FindOnPath("say") is not null)
            {
                providers.Add(new MacSayVoiceFactoryProvider());
            }

            if (OperatingSystem.IsLinux() && ProcessRunner.FindOnPath("espeak-ng") is not null)
            {
                providers.Add(new EspeakNgVoiceFactoryProvider());
            }

            if (OperatingSystem.IsLinux() && ProcessRunner.FindOnPath("pico2wave") is not null)
            {
                providers.Add(new Pico2WaveVoiceFactoryProvider());
            }

            providers.Add(new EdgeTtsVoiceFactoryProvider());

            var aggregator = new VoiceAggregator(providers.ToArray());

            var initialVoice = NullVoice.Instance;

            if (WaitingTimeParser.TryParse(appSettings.Dictation.IntervalExpression, out var calc))
            {
            }

            desktop.MainWindow = new MainWindow(appSettings)
            {
                DataContext = new MainWindowViewModel(wordListSource, audioPlayer, new LocalTextFileService(), aggregator, initialVoice, appSettings)
            };

            if (WaitingTimeParser.TryParse(appSettings.Dictation.IntervalExpression, out var calculator))
            {
                ((MainWindowViewModel)desktop.MainWindow.DataContext!).DictationPlayer.WaitingTimeCalculator = calculator;
            }

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
