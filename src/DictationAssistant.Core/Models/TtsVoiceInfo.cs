namespace DictationAssistant.Core.Models;

public sealed class TtsVoiceInfo
{
    public string Name { get; init; } = string.Empty;

    public string? LocaleOrLanguage { get; init; }
}
