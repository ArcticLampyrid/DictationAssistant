using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Abstractions;

public interface IVoiceFactory
{
    VoiceInfo Info { get; }
    IVoice Create();
}
