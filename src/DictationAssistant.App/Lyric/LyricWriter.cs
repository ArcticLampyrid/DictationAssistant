using System.Text;

namespace DictationAssistant.App.Lyric;

public class LyricWriter : ILyricWriter
{
    private readonly Dictionary<string, SortedSet<long>> _lyric = [];
    private readonly Dictionary<string, string> _idTag = [];

    public LyricWriter()
    {
    }

    public void WriteMetadata(string title, string artist)
    {
        _idTag["title"] = title;
        _idTag["artist"] = artist;
    }

    public void WriteTimestamp(long ms, string text)
    {
        if (!_lyric.ContainsKey(text))
            _lyric.Add(text, []);
        _lyric[text].Add(ms);
    }

    public void SaveTo(Stream stream)
    {
        using var writer = new StreamWriter(stream, Encoding.Default, 1024, true);
        foreach (var pair in _idTag)
            writer.WriteLine($"[{pair.Key}:{pair.Value}]");
        foreach (var pair in _lyric)
        {
            foreach (var offset in pair.Value)
            {
                var min = offset / 60000;
                var sec = (offset % 60000) / (double)1000;
                writer.Write($"[{min:D2}:{sec:00.00}]");
            }
            writer.WriteLine(pair.Key);
        }
    }
}
