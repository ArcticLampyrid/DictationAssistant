using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DictationAssistant
{
    public partial class AddWordDialog : Form
    {
        private ListView WordListView;

        public AddWordDialog(ListView wordListView)
        {
            InitializeComponent();
            WordListView = wordListView ?? throw new ArgumentNullException(nameof(wordListView));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text?.Trim()))
            {
                Close();
                return;
            }
            WordListView.Items.Add(textBox1.Text.Trim());
            textBox1.Text = "";
        }
    }
}
