using System.Collections.Generic;

namespace DictationAssistant
{
    public interface IWordListAdapter
    {
        string this[int index] { get; }
        int Length { get; }
    }
}
