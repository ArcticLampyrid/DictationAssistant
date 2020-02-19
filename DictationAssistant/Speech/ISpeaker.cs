using System;

namespace DictationAssistant.Speech
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
