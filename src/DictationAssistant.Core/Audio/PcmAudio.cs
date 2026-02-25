namespace DictationAssistant.Core.Audio;

public sealed class PcmAudio
{
    public required byte[] Data { get; init; }

    public required PcmFormatInfo Format { get; init; }
}
