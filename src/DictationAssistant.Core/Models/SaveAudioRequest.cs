namespace DictationAssistant.Core.Models;

public sealed class SaveAudioRequest
{
    public required string OutputPath { get; init; }

    public string? LyricsOutputPath { get; init; }
}
