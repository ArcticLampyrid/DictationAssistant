using System;

namespace DictationAssistant
{
    public interface ISpeaker
    {
        ISpeakStateControler Speak(string text);
    }

    public interface ISpeakStateControler
    {
        void StopSpeak();
        event EventHandler PlayCompleted;
    }
}
