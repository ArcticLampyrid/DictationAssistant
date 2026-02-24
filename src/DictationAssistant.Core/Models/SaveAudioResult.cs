namespace DictationAssistant.Core.Models;

public sealed record SaveAudioResult(bool Succeeded, string Message)
{
    public static SaveAudioResult Success(string message) => new(true, message);

    public static SaveAudioResult NotSupported(string message) => new(false, message);
}
