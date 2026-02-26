using DictationAssistant.Abstractions;

namespace DictationAssistant.Services;

public sealed class WordListDocument : IWordListSource
{
    private readonly List<string> _lines = [];

    public WordListDocument(string text = "")
    {
        SetText(text);
    }

    public int Count => _lines.Count;

    public event EventHandler? Changed;

    public string GetWordAt(int index) => _lines[index];

    public IReadOnlyList<string> GetWords() => _lines;

    public string GetText() => string.Join(Environment.NewLine, _lines);

    public void SetText(string text)
    {
        _lines.Clear();
        if (!string.IsNullOrEmpty(text))
        {
            var normalized = text.Replace("\r\n", "\n");
            _lines.AddRange(normalized.Split('\n'));
        }

        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void SetLines(IEnumerable<string> lines)
    {
        _lines.Clear();
        _lines.AddRange(lines);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
