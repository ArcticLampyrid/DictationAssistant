using System.Windows.Forms;
using Microsoft.VisualBasic;
using System;

namespace DictationAssistant
{
    public partial class SetImprovedSpeakEngineDialog
    {
        public SetImprovedSpeakEngineDialog()
        {
            InitializeComponent();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                TextBox1.Text = FolderBrowserDialog1.SelectedPath;
        }

        private void SetImprovedSpeakEngineDialog_Load(object sender, EventArgs e)
        {
            if (Module1.CiZuZengQiangMuLu == "")
                CheckBox1.Checked = false;
            else
                CheckBox1.Checked = true;
            TextBox1.Text = Module1.CiZuZengQiangMuLu;
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            if (!CheckBox1.Checked)
            {
                Module1.CiZuZengQiangMuLu = "";
            }
            else if (System.IO.Directory.Exists(TextBox1.Text) == false)
                MessageBox.Show("不存在该文件夹！");
            else
            {
                Module1.CiZuZengQiangMuLu = TextBox1.Text;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (TextBox1.Text != "")
                CheckBox1.Checked = true;
            else
                CheckBox1.Checked = false;
        }
    }
}
