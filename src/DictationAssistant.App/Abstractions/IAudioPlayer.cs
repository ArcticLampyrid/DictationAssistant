using DictationAssistant.App.Audio;

namespace DictationAssistant.App.Abstractions;

public interface IAudioPlayer
{
    Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
