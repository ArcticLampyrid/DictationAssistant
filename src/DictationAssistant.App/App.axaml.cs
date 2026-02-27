using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DictationAssistant.App.Services;
using DictationAssistant.App.Audio;
using DictationAssistant.App.Settings;
using DictationAssistant.App.Voice;
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
                DataContext = new MainWindowViewModel(wordListSource, audioPlayer, new LocalTextFileService(), aggregator, initialVoice, appSettings, settingsStore)
            };

            if (WaitingTimeParser.TryParse(appSettings.Dictation.IntervalExpression, out var calculator))
            {
                ((MainWindowViewModel)desktop.MainWindow.DataContext!).DictationPlayer.WaitingTimeCalculator = calculator;
            }

            desktop.MainWindow.Closing += (_, _) => settingsStore.Save(appSettings);

            // Command-line file argument support (v3.x compat)
            if (desktop.Args is { Length: > 0 } cliArgs)
            {
                var filePath = cliArgs[0];
                if (File.Exists(filePath) && desktop.MainWindow is MainWindow mainWin)
                {
                    mainWin.Opened += async (_, _) =>
                    {
                        await mainWin.LoadFileByPathAsync(Path.GetFullPath(filePath));
                    };
                }
            }

            desktop.Exit += (_, _) =>
            {
                settingsStore.Save(appSettings);
                audioPlayer.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
