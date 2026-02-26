using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using DictationAssistant.App.Abstractions;
using DictationAssistant.App.ViewModels;

namespace DictationAssistant.App.Services;

public sealed class EditableWordListSource : IWordListSource
{
    public EditableWordListSource()
    {
        Lines.CollectionChanged += LinesOnCollectionChanged;
    }

    public ObservableCollection<WordLineViewModel> Lines { get; } = [];

    public int Count => Lines.Count;

    public event EventHandler? Changed;

    public IReadOnlyList<string> GetWords() => Lines.Select(x => x.Text).ToList();

    public string GetWordAt(int index) => Lines[index].Text;

    public void ReplaceLines(IEnumerable<string> lines)
    {
        foreach (var item in Lines)
        {
            item.PropertyChanged -= LineOnPropertyChanged;
        }

        Lines.Clear();
        foreach (var line in lines)
        {
            var item = new WordLineViewModel { Text = line };
            item.PropertyChanged += LineOnPropertyChanged;
            Lines.Add(item);
        }

        Reindex();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public string ToText() => string.Join(Environment.NewLine, Lines.Select(x => x.Text));

    private void LinesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (var oldItem in e.OldItems.OfType<WordLineViewModel>())
            {
                oldItem.PropertyChanged -= LineOnPropertyChanged;
            }
        }

        if (e.NewItems is not null)
        {
            foreach (var newItem in e.NewItems.OfType<WordLineViewModel>())
            {
                newItem.PropertyChanged += LineOnPropertyChanged;
            }
        }

        Reindex();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void LineOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(WordLineViewModel.Text))
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Reindex()
    {
        for (var i = 0; i < Lines.Count; i++)
        {
            Lines[i].LineNumber = i + 1;
        }
    }
}
