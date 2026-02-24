using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public static class TtsEngineFactory
{
    public static ITtsEngine CreateDefault()
    {
        if (OperatingSystem.IsWindows())
        {
            if (ProcessRunner.FindOnPath("powershell") is not null)
            {
                return new WindowsSystemSpeechTtsEngine();
            }

            return new NullTtsEngine();
        }

        if (OperatingSystem.IsMacOS())
        {
            if (ProcessRunner.FindOnPath("say") is not null)
            {
                return new MacSayTtsEngine();
            }

            return new NullTtsEngine();
        }

        if (OperatingSystem.IsLinux())
        {
            if (ProcessRunner.FindOnPath("espeak-ng") is not null)
            {
                return new LinuxEspeakNgTtsEngine();
            }

            if (ProcessRunner.FindOnPath("pico2wave") is not null)
            {
                var playback = ProcessRunner.FindOnPath("aplay") is not null
                    ? "aplay"
                    : ProcessRunner.FindOnPath("paplay") is not null
                        ? "paplay"
                        : string.Empty;

                if (!string.IsNullOrEmpty(playback))
                {
                    return new LinuxPico2WaveTtsEngine(playback);
                }
            }

            return new NullTtsEngine();
        }

        return new NullTtsEngine();
    }
}
