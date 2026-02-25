using DictationAssistant.Core.Audio;

namespace DictationAssistant.Core.Abstractions;

public interface IAudioPlayer
{
    Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
