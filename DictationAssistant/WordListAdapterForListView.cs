using System.Windows.Forms;

namespace DictationAssistant
{
    public class WordListAdapterForListView : IWordListAdapter
    {
        private delegate void ActionWithoutArgs();
        private delegate T FuncWithoutArgs<out T>();

        private ListView ListViewObject;
        public WordListAdapterForListView(ListView listViewObject)
        {
            this.ListViewObject = listViewObject;
        }

        public string this[int index] => System.Convert.ToString(ListViewObject.Invoke(new FuncWithoutArgs<string>(() => ListViewObject.Items[index].Text)));

        public int Length => (int)ListViewObject.Invoke(new FuncWithoutArgs<int>(() => ListViewObject.Items.Count));
    }
}
