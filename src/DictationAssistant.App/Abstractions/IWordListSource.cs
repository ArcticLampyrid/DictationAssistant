namespace DictationAssistant.Abstractions;

public interface IWordListSource
{
    int Count { get; }

    string GetWordAt(int index);

    IReadOnlyList<string> GetWords();

    event EventHandler? Changed;
}
