using ICSharpCode.AvalonEdit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictationAssistant
{
    class WordListAdapterForAvalonEdit
        : IWordListAdapter
    {
        private readonly TextEditor textEditor;

        public WordListAdapterForAvalonEdit(TextEditor textEditor)
        {
            this.textEditor = textEditor;
        }

        public string this[int index]
        {
            get
            {
                return (string)textEditor.Dispatcher.Invoke(new Func<string>(() => {
                    var documentLine = textEditor.Document.GetLineByNumber(index + 1);
                    return textEditor.Document.GetText(documentLine.Offset, documentLine.Length);
                }));
            }
        }

        public int Length => (int)textEditor.Dispatcher.Invoke(new Func<int>(() => this.textEditor.LineCount));
    }
}
