namespace DictationAssistant.App.Models;

public sealed class SaveAudioRequest
{
    public required string OutputPath { get; init; }

    public string? LyricsOutputPath { get; init; }

    public int SampleRate { get; init; } = 44100;

    public int Channels { get; init; } = 2;

    public string OutputFormat { get; init; } = "wav";

    public string? LyricMode { get; init; }

    public IProgress<double>? Progress { get; init; }
}
