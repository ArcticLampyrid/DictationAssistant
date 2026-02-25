using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public static class TtsEngineFactory
{
    public static IPcmTtsEngine CreateDefaultPcmEngine()
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindowsSapiComPcmTtsEngine();
        }

        if (OperatingSystem.IsMacOS())
        {
            if (ProcessRunner.FindOnPath("say") is not null)
            {
                return new MacSayTtsEngine();
            }

            return new NullPcmTtsEngine();
        }

        if (OperatingSystem.IsLinux())
        {
            if (ProcessRunner.FindOnPath("espeak-ng") is not null)
            {
                return new LinuxEspeakNgTtsEngine();
            }

            if (ProcessRunner.FindOnPath("pico2wave") is not null)
            {
                return new LinuxPico2WaveTtsEngine();
            }

            return new NullPcmTtsEngine();
        }

        return new NullPcmTtsEngine();
    }

    public static ITtsEngine CreateDefault()
    {
        return CreateDefaultPcmEngine() as ITtsEngine ?? new NullTtsEngine();
    }
}
