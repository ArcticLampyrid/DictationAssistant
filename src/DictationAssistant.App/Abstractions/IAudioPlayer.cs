using DictationAssistant.Audio;

namespace DictationAssistant.Abstractions;

public interface IAudioPlayer
{
    Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
