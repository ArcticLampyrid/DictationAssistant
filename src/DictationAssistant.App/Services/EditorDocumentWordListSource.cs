using AvaloniaEdit.Document;
using DictationAssistant.App.Abstractions;

namespace DictationAssistant.App.Services;

public sealed class EditorDocumentWordListSource : IWordListSource
{
    private TextDocument? _document;
    private string[] _lines = [string.Empty];

    public int Count => _lines.Length;

    public event EventHandler? Changed;

    public void AttachDocument(TextDocument document)
    {
        if (ReferenceEquals(_document, document))
        {
            return;
        }

        if (_document is not null)
        {
            _document.Changed -= OnDocumentChanged;
        }

        _document = document;
        _document.Changed += OnDocumentChanged;
        RefreshLines();
    }

    public IReadOnlyList<string> GetWords() => _lines;

    public string GetWordAt(int index) => _lines[index];

    private void OnDocumentChanged(object? sender, DocumentChangeEventArgs e)
    {
        _ = sender;
        _ = e;
        RefreshLines();
    }

    private void RefreshLines()
    {
        var normalized = (_document?.Text ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n');
        _lines = normalized.Split('\n');
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
