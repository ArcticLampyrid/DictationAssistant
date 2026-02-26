using DictationAssistant.App.Abstractions;

namespace DictationAssistant.App.Lyric;

public interface ILyricWriter : IDisposable
{
    void WriteMetadata(string title, string artist);
    void WriteTimestamp(long ms, string text);
    void WriteAllTimestamp(IReadOnlyList<string> words, long startTime, IWaitingTimeCalculator waitingTimeCalculator);
    void Flush();
}
