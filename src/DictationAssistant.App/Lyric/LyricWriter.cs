using System.Collections.Generic;
using System.IO;
using System.Text;
using DictationAssistant.Abstractions;

namespace DictationAssistant.App.Lyric;

public class LyricWriter : ILyricWriter
{
    private readonly Dictionary<string, Dictionary<long, object>> _lyric = new();
    private readonly Dictionary<string, string> _idTag = new();
    private readonly string _fileName;
    private bool _disposed;

    public LyricWriter(string fileName)
    {
        _fileName = fileName;
    }

    public void WriteMetadata(string title, string artist)
    {
        _idTag["title"] = title;
        _idTag["artist"] = artist;
    }

    public void WriteTimestamp(long ms, string text)
    {
        if (!_lyric.ContainsKey(text))
            _lyric.Add(text, new Dictionary<long, object>());
        _lyric[text].Add(ms, new object());
    }

    public void WriteAllTimestamp(IReadOnlyList<string> words, long startTime, IWaitingTimeCalculator waitingTimeCalculator)
    {
        long currentTime = startTime;
        foreach (var word in words)
        {
            WriteTimestamp(currentTime, word);
            var waitingTime = waitingTimeCalculator.CalculateWaitingTime(word);
            currentTime += waitingTime * 1000;
        }
    }

    public void Flush()
    {
        using var writer = new StreamWriter(File.Open(_fileName, FileMode.Create), Encoding.Default);
        foreach (var pair in _idTag)
            writer.WriteLine($"[{pair.Key}:{pair.Value}]");
        foreach (var pair in _lyric)
        {
            foreach (var offset in pair.Value.Keys)
            {
                var min = offset / 60000;
                var sec = (offset % 60000) / (double)1000;
                writer.Write($"[{min:D2}:{sec:00.00}]");
            }
            writer.WriteLine(pair.Key);
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Flush();
            _disposed = true;
        }
    }
}
