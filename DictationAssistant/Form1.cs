using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DictationAssistant
{
    public partial class Form1
    {
        private delegate void ActionWithoutArgs();

        public Form1()
        {
            InitializeComponent();
            BoBao = new SpeakControler();
        }

        private List<ISpeakEngine> SpeakEngines = new List<ISpeakEngine>();
        private SpeakControler BoBao;


        /// <summary>
    /// 打开多个词语文件
    /// </summary>
    /// <param name="Files">词语文件列表（Ansi编码）</param>
    /// <remarks>不会在列表中移除原词语</remarks>
        public void OpenFiles(string[] Files)
        {
            WordListView.BeginUpdate();

            foreach (string FileName in Files)
            {
                using (StreamReader ReadText = new StreamReader(FileName, System.Text.Encoding.Default))
                {
                    while (!ReadText.EndOfStream)
                    {
                        string CiYu = ReadText.ReadLine();
                        if (CiYu.Trim() != "")
                            WordListView.Items.Add(CiYu);
                    }
                }
            }

            WordListView.EndUpdate();
        }
        /// <summary>
    /// 保存词语文件
    /// </summary>
    /// <param name="FileName">文件路径</param>
    /// <remarks></remarks>
        public void SaveFile(string FileName)
        {
            using (StreamWriter WriterText = new StreamWriter(FileName, false, System.Text.Encoding.Default))
            {
                foreach (ListViewItem ListItem in WordListView.Items)
                    WriterText.WriteLine(ListItem.Text);
            }
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenFiles(OpenFileDialog1.FileNames);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            WordListView.BeginUpdate();
            WordListView.Items.Clear();
            WordListView.EndUpdate();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            WordListView.BeginUpdate();
            foreach (ListViewItem SelectedItem in WordListView.SelectedItems)
                SelectedItem.Remove();
            WordListView.EndUpdate();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            new AddWordDialog(WordListView).ShowDialog(this);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (WordListView.SelectedItems.Count > 0)
                WordListView.SelectedItems[0].BeginEdit();
        }
        private void UpButton_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SelectedItems = WordListView.SelectedItems;
            if (SelectedItems.Count > 0)
            {
                if (SelectedItems[0].Index > 0)
                {
                    WordListView.BeginUpdate();

                    foreach (ListViewItem SelectedItem in SelectedItems)
                    {
                        int SelectedItemIndex = SelectedItem.Index;
                        SelectedItem.Remove();
                        WordListView.Items.Insert(SelectedItemIndex - 1, SelectedItem).EnsureVisible();
                    }

                    WordListView.EndUpdate();
                }
            }
        }
        private void DownButton_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SelectedItems = WordListView.SelectedItems;
            if (SelectedItems.Count > 0)
            {
                if (SelectedItems[SelectedItems.Count - 1].Index < WordListView.Items.Count - 1)
                {
                    WordListView.BeginUpdate();

                    for (var i = SelectedItems.Count - 1; i >= 0; i += -1)
                    {
                        ListViewItem SelectedItem = SelectedItems[i];
                        int SelectedItemIndex = SelectedItem.Index;
                        SelectedItem.Remove();
                        WordListView.Items.Insert(SelectedItemIndex + 1, SelectedItem).EnsureVisible();
                    }

                    WordListView.EndUpdate();
                }
            }
        }
        private void CountButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("词语数量：" + WordListView.Items.Count.ToString());
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenButton_Click(OpenButton, EventArgs.Empty);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SaveFile(SaveFileDialog1.FileName);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (WordListView.Items.Count == 0)
            {
                MessageBox.Show("请先添加词语！");
                return;
            }
            if (BoBao.NextWordIndex>= WordListView.Items.Count)
            {
                MessageBox.Show("已经播完了。");
                return;
            }
            Button3.Text = "暂停自动播报(&P)";
            BoBao.SpeakNext();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            My.Settings.Default.Engine = ComboBox1.SelectedIndex;
            My.Settings.Default.自动翻页 = 自动翻页ToolStripMenuItem.Checked;
            My.Settings.Default.字词跟随 = 字词跟随ToolStripMenuItem.Checked;
            My.Settings.Default.Volume = TrackBar1.Value;
            My.Settings.Default.Rate = TrackBar2.Value;
            My.Settings.Default.Font = WordListView.Font;
            My.Settings.Default.ForeColor = WordListView.ForeColor;
            My.Settings.Default.词组增强 = Module1.CiZuZengQiangMuLu;
            My.Settings.Default.Save();
            
            BoBao.StopThis();
            BASS.FreeAllPlugin();
            BASS.Free();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (BASS.Init(0, 44100, 0, Handle))
            {
                try
                {
                    DirectoryInfo pluginDir = new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "bass_plugin");
                    foreach (var File in pluginDir.GetFiles())
                        BASS.LoadPlugin(File.FullName);
                }
                catch (Exception)
                {
                }
                Module1.CiZuZengQiangMuLu = My.Settings.Default.词组增强;
            }
            else
            {
                设置词组增强ToolStripMenuItem.Enabled = false;
                设置词组增强ToolStripMenuItem.Text = "加载词组增强功能失败";
            }
            BoBao.Speaker = new Speaker();
            BoBao.WordListSource = new WordListAdapterForListView(WordListView);
            BoBao.StoppedThis += BoBao_StoppedThis;
            BoBao.Speaking += BoBao_Speaking;
            BoBao.UpdateWaitingTimeInfo += BoBao_UpdateWaitingTimeInfo;
            BoBao.AutoSpeakingCompleted += BoBao_AutoSpeakingCompleted;

            InitVoiceList();
            try
            {
                ComboBox1.SelectedIndex = My.Settings.Default.Engine;
            }
            catch (Exception)
            {
                if (ComboBox1.Items.Count > 0)
                    ComboBox1.SelectedIndex = 0;
            }

            WordListView.Font = My.Settings.Default.Font;
            WordListView.ForeColor = My.Settings.Default.ForeColor;
            自动翻页ToolStripMenuItem.Checked = My.Settings.Default.自动翻页;
            字词跟随ToolStripMenuItem.Checked = My.Settings.Default.字词跟随;

            TrackBar1.Value = My.Settings.Default.Volume;
            TrackBar1_Scroll(TrackBar1, EventArgs.Empty);

            TrackBar2.Value = My.Settings.Default.Rate;
            TrackBar2_Scroll(TrackBar2, EventArgs.Empty);

            ComboBox1_SelectedIndexChanged(ComboBox1, EventArgs.Empty);
            try
            {
                string[] cmd = Environment.GetCommandLineArgs();
                for (int i = 1; i < cmd.Length; i++)
                {
                    string CommandLine = cmd[i];
                    if (File.Exists(CommandLine))
                        OpenFiles(new[] { CommandLine });
                }
            }
            catch (Exception)
            {
            }

        }
        private void InitVoiceList()
        {
            foreach (var NativeEngine in MicrosoftSpeechEngine.GetAllNativeEngines())
                SpeakEngines.Add(new MicrosoftSpeechEngine(NativeEngine));

            int EnglishEngineLevel = 0; // 用于记录英文语音引擎等级，越高表示引擎越好
            int ChineseEngineLevel = 0; // 用于记录中文语音引擎等级，越高表示引擎越好
            foreach (ISpeakEngine SpeakEngine in SpeakEngines)
            {
                string EngineName = SpeakEngine.Name;
                if (SpeakEngine.Culture != null)
                {
                    if (SpeakEngine.Culture.LCID == 1033 | SpeakEngine.Culture.LCID == 2057)
                    {
                        if (Button11.Tag == null || EnglishEngineLevel < Module1.GetEnglishEngineLevel(EngineName))
                        {
                            Button11.Tag = (ComboBox1.Items.Count);
                            EnglishEngineLevel = Module1.GetEnglishEngineLevel(EngineName);
                        }
                        if (SpeakEngine.Culture.LCID == 1033)
                            EngineName = "（US）" + EngineName;
                        else if (SpeakEngine.Culture.LCID == 2057)
                            EngineName = "（UK）" + EngineName;
                    }
                    else if (SpeakEngine.Culture.LCID == 2052)
                    {
                        if (Button10.Tag == null || ChineseEngineLevel < Module1.GetChineseEngineLevel(EngineName))
                        {
                            Button10.Tag = (ComboBox1.Items.Count);
                            ChineseEngineLevel = Module1.GetChineseEngineLevel(EngineName);
                        }
                        EngineName = "（CN）" + EngineName;
                    }
                }
                ComboBox1.Items.Add(EngineName);
            }
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            if ((BoBao.NextWordIndex<= WordListView.Items.Count) & (BoBao.NextWordIndex> 0))
            {
                Button3.Text = "暂停自动播报(&P)";
                BoBao.SpeakAgain();
            }
            else
                MessageBox.Show(this, "还没报过或已移除报过的词语！");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (BoBao.NextWordIndex<= WordListView.Items.Count & BoBao.NextWordIndex> 1)
            {
                Button3.Text = "暂停自动播报(&P)";
                BoBao.SpeakLast();
            }
            else
                MessageBox.Show(this, "已经是第1个了！");
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (!BoBao.IsAutoMode)
            {
                BoBao.ResetProgress();
                WordListView.BeginUpdate();
                foreach (ListViewItem ListItem in WordListView.Items)
                    ListItem.BackColor = Color.Empty;
                WordListView.EndUpdate();
            }
            else
                MessageBox.Show(this, "请先停止自动播报！");
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            if (WordListView.Items.Count == 0)
                return;
            if (BoBao.IsAutoMode)
                BoBao.StopAuto();
            Button5_Click(Button1, EventArgs.Empty); // 记录归零
            BoBao.StartAuto();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (BoBao.IsAutoMode)
                BoBao.StopAuto();
            else if (BoBao.IsSpeaking)
                BoBao.StopThis();
            else
                return;
            BoBao_StoppedThis(BoBao.NextWordIndex- 1, false);
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveButton_Click(保存SToolStripMenuItem, EventArgs.Empty);
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (Button9.Text == "隐藏词语列表")
            {
                WordListView.Visible = false;
                OpenButton.Visible = false;
                SaveButton.Visible = false;
                UpButton.Visible = false;
                DownButton.Visible = false;
                AddButton.Visible = false;
                EditButton.Visible = false;
                DeleteButton.Visible = false;
                ClearButton.Visible = false;
                CountButton.Visible = false;
                Label5.Visible = true;
                Button9.Text = "显示词语列表";
            }
            else
            {
                WordListView.Visible = true;
                OpenButton.Visible = true;
                SaveButton.Visible = true;
                UpButton.Visible = true;
                DownButton.Visible = true;
                AddButton.Visible = true;
                EditButton.Visible = true;
                DeleteButton.Visible = true;
                ClearButton.Visible = true;
                CountButton.Visible = true;
                Label5.Visible = false;
                Button9.Text = "隐藏词语列表";
            }
        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(上移ToolStripMenuItem, EventArgs.Empty);
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownButton_Click(下移ToolStripMenuItem, EventArgs.Empty);
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditButton_Click(修改ToolStripMenuItem, EventArgs.Empty);
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteButton_Click(移除ToolStripMenuItem, EventArgs.Empty);
        }

        private void 反向选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            反向选择ToolStripMenuItem1_Click(反向选择ToolStripMenuItem, EventArgs.Empty);
        }

        private void 显示隐藏词语列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button9_Click(显示隐藏词语列表ToolStripMenuItem, EventArgs.Empty);
        }

        private void 添加词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton_Click(添加词语ToolStripMenuItem, EventArgs.Empty);
        }

        private void 移除选中词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteButton_Click(移除选中词语ToolStripMenuItem, EventArgs.Empty);
        }

        private void 清空词语列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearButton_Click(清空词语列表ToolStripMenuItem, EventArgs.Empty);
        }

        private void 修改选中词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditButton_Click(修改选中词语ToolStripMenuItem, EventArgs.Empty);
        }

        private void 向上移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(向上移动ToolStripMenuItem, EventArgs.Empty);
        }

        private void 向下移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownButton_Click(向下移动ToolStripMenuItem, EventArgs.Empty);
        }

        private void 反向选择ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem ListItem in WordListView.Items)
                ListItem.Selected = !ListItem.Selected;
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (Button10.Tag == null)
            {
                MessageBox.Show("未发现中文语音引擎！");
                return;
            }
            ComboBox1.SelectedIndex = System.Convert.ToInt32(Button10.Tag);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if (Button11.Tag == null)
            {
                MessageBox.Show("未发现英文语音引擎！");
                return;
            }
            ComboBox1.SelectedIndex = System.Convert.ToInt32(Button11.Tag);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ISpeakEngine Engine = null;
            if (ComboBox1.SelectedIndex != -1)
                Engine = SpeakEngines[ComboBox1.SelectedIndex];
            if (Module1.CiZuZengQiangMuLu != null && Module1.CiZuZengQiangMuLu.Trim() != "")
                Engine = new ImprovedSpeakEngine(Engine, Module1.CiZuZengQiangMuLu);
            ((Speaker)BoBao.Speaker).Engine = Engine;
        }

        private void 设置词组增强ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new SetImprovedSpeakEngineDialog().ShowDialog() == DialogResult.OK)
                ComboBox1_SelectedIndexChanged(ComboBox1, EventArgs.Empty);
        }

        private void 自动翻页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自动翻页ToolStripMenuItem.Checked = !自动翻页ToolStripMenuItem.Checked;
        }

        private void 字词跟随ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            字词跟随ToolStripMenuItem.Checked = !字词跟随ToolStripMenuItem.Checked;
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            foreach (var SpeakEngineObject in SpeakEngines)
                ((Speaker)BoBao.Speaker).Volume = System.Convert.ToByte(TrackBar1.Value);
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            foreach (var SpeakEngineObject in SpeakEngines)
                ((Speaker)BoBao.Speaker).Rate = System.Convert.ToSByte(TrackBar2.Value);
        }


        private void 全选ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            全选ToolStripMenuItem_Click(全选ToolStripMenuItem1, EventArgs.Empty);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem ListItem in WordListView.Items)
                ListItem.Selected = true;
        }

        private void ZiDongBoBaoJianGeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ZiDongBoBaoJianGeTextBox.Text == "")
            {
                ZiDongBoBaoJianGeTextBox.Text = "0";
                ZiDongBoBaoJianGeTextBox.Select(0, 1);
            }

            IWaitingTimeCalculator WaitingTimeCalculator;
            try
            {
                WaitingTimeCalculator = new WaitingTimeAccordingToExpression(ZiDongBoBaoJianGeTextBox.Text);
                WaitingTimeCalculator.CalculateWaitingTime("测试 Test");
                ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "");
            }
            catch (Exception)
            {
                WaitingTimeCalculator = new FixedWaitingTime(0);
                ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "表达式有误");
            }
            BoBao.CurrentWaitingTimeCalculator = WaitingTimeCalculator;
        }

        private void ZiDongBoBaoCiShuTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) | e.KeyChar == '\x08')
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void ZiDongBoBaoCiShuTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ZiDongBoBaoCiShuTextBox.Text == "" || System.Convert.ToInt32(ZiDongBoBaoCiShuTextBox.Text) < 1)
            {
                ZiDongBoBaoCiShuTextBox.Text = "1";
                ZiDongBoBaoCiShuTextBox.Select(0, 1);
            }
            BoBao.AutoMode_TimesPerWord = Convert.ToInt32(ZiDongBoBaoCiShuTextBox.Text);
        }

        private void 随机排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WordListView.BeginUpdate();

            var rand = new Random();
            for (int EndIndex = WordListView.Items.Count; EndIndex >= 2; EndIndex += -1)
            {
                ListViewItem ListItem;
                int xxczdsyh;
                xxczdsyh = rand.Next(0, EndIndex);
                ListItem = WordListView.Items[xxczdsyh];
                ListItem.Remove();
                WordListView.Items.Add(ListItem);
            }

            WordListView.EndUpdate();
        }

        private void 随机选词ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RandomExtractionForm(WordListView).Show(this);
        }


        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult FontReturnValue;

            FontDialog1.Font = WordListView.Font;
            FontDialog1.Color = WordListView.ForeColor;

            FontReturnValue = FontDialog1.ShowDialog();
            if (FontReturnValue == DialogResult.OK)
            {
                WordListView.Font = FontDialog1.Font;
                WordListView.ForeColor = FontDialog1.Color;
            }
        }

        private void 读选定词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedIndex;
            if (WordListView.SelectedIndices.Count > 0)
            {
                SelectedIndex = WordListView.SelectedIndices[0];
                Button3.Text = "暂停自动播报(&P)";
                BoBao.Speak(SelectedIndex);
            }
        }
        private void WordListView_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            移除ToolStripMenuItem.Enabled = false;
        }
        private void WordListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Trim() == "")
                    e.CancelEdit = true;
            }
            移除ToolStripMenuItem.Enabled = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (BoBao.IsAutoMode == false)
            {
                MessageBox.Show("当前不在自动播报！");
                return;
            }
            if (Button3.Text == "暂停自动播报(&P)")
            {
                BoBao.PauseAuto();
                Label4.Text = "自动播报已暂停";
                Button3.Text = "继续自动播报(&R)";
            }
            else
            {
                BoBao.ResumeAuto();
                Button3.Text = "暂停自动播报(&P)";
            }
        }

        private void BoBao_StoppedThis(int i, bool completed)
        {
            WordListView.Invoke(new ActionWithoutArgs(() =>
            {
                读选定词语ToolStripMenuItem.Enabled = true;
                if (!BoBao.IsAutoMode)
                    Label4.Text = "等待播报";
                try
                {
                    Label1.Text = "本词语播报次数：" + BoBao.ElapsedTimes;
                }
                catch (Exception)
                {
                    Label1.Text = "本词语播报次数：0";
                }
            }));
        }

        private void BoBao_Speaking(int i)
        {
            WordListView.Invoke(new ActionWithoutArgs(() =>
            {
                Label4.Text = "正在播报第" + (i + 1).ToString() + "个";
                Label1.Text = "本词语播报次数：" + BoBao.ElapsedTimes;
                if (字词跟随ToolStripMenuItem.Checked == true)
                {
                    var BackColor = Color.FromArgb(0x33, 0x66, 0xFF);
                    if (WordListView.Items[i].BackColor != BackColor)
                    {
                        foreach (ListViewItem ListItem in WordListView.Items)
                        {
                            if (ListItem.BackColor == BackColor)
                                ListItem.BackColor = Color.Empty;
                        }
                        WordListView.Items[i].BackColor = BackColor;
                    }
                }
                if (自动翻页ToolStripMenuItem.Checked == true)
                    WordListView.Items[i].EnsureVisible();
            }));
        }

        private void BoBao_UpdateWaitingTimeInfo(int i, int jiangge)
        {
            WordListView.Invoke(new ActionWithoutArgs(() => Label4.Text = jiangge.ToString() + "秒后自动播报第" + (i + 1).ToString() + "个"));
        }

        private void BoBao_AutoSpeakingCompleted(bool completed)
        {
            WordListView.Invoke(new ActionWithoutArgs(() =>
            {
                Button4.Enabled = true;
                Button8.Enabled = true;
                Button7.Enabled = true;
                读选定词语ToolStripMenuItem.Enabled = true;
                Label4.Text = "等待播报";
                if (completed)
                    MessageBox.Show(this, "自动播报已完成！");
                Button3.Text = "暂停自动播报(&P)";
            }));
        }

        private void 在Bing词典中查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedItemIndex;
            string SelectedItemText;
            if (WordListView.SelectedIndices.Count > 0)
            {
                SelectedItemIndex = WordListView.SelectedIndices[0];
                SelectedItemText = WordListView.Items[SelectedItemIndex].Text;
                Process.Start("https://cn.bing.com/dict/search?q=" + UrlEncode_UTF8(SelectedItemText) + "&qs=n");
            }
        }
        public static string UrlEncode_UTF8(string s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            foreach (byte b in bytes)
                sb.AppendFormat("%{0:X2}", b);
            return sb.ToString();
        }

        private void WordListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                OpenFiles(files);
            }
        }

        private void WordListView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.Undo();
        }
        private void ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.Cut();
        }
        private void ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.Copy();
        }
        private void ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.Paste();
        }
        private void ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.SelectedText = "";
        }
        private void ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.SelectAll();
        }
        private void 插入LengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.SelectedText = "[Length]";
        }
        private void 插入WordCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZiDongBoBaoJianGeTextBox.SelectedText = "[WordCount]";
        }

        private void 保存音频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SaveAudioDialog(BoBao).ShowDialog(this);
        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            if (WordListView.SelectedItems.Count > 0)
            {
                item = WordListView.Items.Insert(WordListView.SelectedItems[0].Index, "Please edit");
            }
            else
            {
                item = WordListView.Items.Add("Please edit");
            }
            item.Selected = true;
            item.BeginEdit();
        }

        private void 从此处开始自动播报ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WordListView.SelectedItems.Count > 0)
            {
                if (BoBao.IsAutoMode)
                    BoBao.StopAuto();
                Button5_Click(Button1, EventArgs.Empty); // 记录归零
                BoBao.StartAuto(WordListView.SelectedItems[0].Index);
            }
        }
    }
}
