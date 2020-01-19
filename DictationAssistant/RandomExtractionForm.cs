using System;
using System.Windows.Forms;

namespace DictationAssistant
{
    public partial class RandomExtractionForm
    {

        private ListView WordListView;

        public RandomExtractionForm(ListView wordListView)
        {
            InitializeComponent();
            this.WordListView = wordListView ?? throw new ArgumentNullException(nameof(wordListView));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var rand = new Random();

            int numOfTarget;
            numOfTarget = Convert.ToInt32(NumericUpDown1.Value);
            while (numOfTarget != WordListView.Items.Count)
                WordListView.Items[rand.Next(0, WordListView.Items.Count)].Remove();// 随机删除一个词语
            this.Close();
        }

        private void RandomExtractionForm_Load(object sender, EventArgs e)
        {
            NumericUpDown1.Maximum = WordListView.Items.Count;
        }
    }
}
