using DictationAssistant.App.Abstractions;

namespace DictationAssistant.App.Lyric;

public interface ILyricWriter
{
    void WriteMetadata(string title, string artist);
    void WriteTimestamp(long ms, string text);
    void SaveTo(Stream stream);
}
