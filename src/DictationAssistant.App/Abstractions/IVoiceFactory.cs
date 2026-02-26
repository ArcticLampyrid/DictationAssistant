using DictationAssistant.Models;

namespace DictationAssistant.Abstractions;

public interface IVoiceFactory
{
    VoiceInfo Info { get; }
    IVoice Create();
}
