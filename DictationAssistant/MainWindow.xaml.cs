using DictationAssistant.Audio;
using DictationAssistant.Speech;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using QIQI.WpfFontPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace DictationAssistant
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<IVoice> voices = new List<IVoice>();
        private readonly SpeakControler controler = new SpeakControler();
        private readonly Speaker speaker = new Speaker();
        private readonly VoiceSettingInfo voiceSettingInfo;
        private readonly HighlightedLineBackgroundRenderer WordHighlighter;

        public MainWindow()
        {
            controler.Speaker = speaker;
            foreach (var NativeVoice in MicrosoftSpeechVoice.AllNativeVoices)
                voices.Add(new MicrosoftSpeechVoice(NativeVoice));
            voiceSettingInfo = new VoiceSettingInfo(speaker);

            InitializeComponent();

            ControlerStatus.DataContext = controler;
            PauseSpeakingButton.DataContext = controler;
            VoiceSettingGroupBox.DataContext = speaker;
            VoiceComboxBox.DataContext = voiceSettingInfo;
            VoiceComboxBox.ItemsSource = voices;

            controler.WordListSource = new WordListAdapterForAvalonEdit(WordlistEditor);
            WordlistEditor.Encoding = Encoding.UTF8;
            using (XmlTextReader reader = new XmlTextReader(new MemoryStream(Properties.Resources.WordlistHighlighting)))
            {
                WordlistEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            voiceSettingInfo.BaseVoice = voices.FirstOrDefault(x => x.Name == Properties.Settings.Default.Voice);
            voiceSettingInfo.PathForImprovedResource = Properties.Settings.Default.PathForImprovedResource;
            Properties.Settings.Default.FontInfo?.ApplyTo(WordlistEditor);

            WordHighlighter = new HighlightedLineBackgroundRenderer(WordlistEditor.TextArea.TextView)
            {
                Background = Brushes.LightGreen
            };

            controler.PropertyChanged += (sender, e) =>
            {
                if(e.PropertyName == nameof(controler.NextWordIndex))
                {
                    var lineNumber = controler.NextWordIndex;
                    WordlistEditor.Dispatcher.Invoke(new Action(() =>
                    {
                        if (HighlightSpeakingLineMenu.IsChecked)
                        {
                            WordHighlighter.LineNumber = lineNumber;
                        }
                        else
                        {
                            WordHighlighter.LineNumber = -1;
                        }
                        if (lineNumber < 1 || lineNumber > WordlistEditor.LineCount)
                            return;
                        if (ScrollLockMenu.IsChecked)
                            WordlistEditor.ScrollToLine(lineNumber);
                    }));
                }
            };

            string[] cmd = Environment.GetCommandLineArgs();
            if(cmd.Length >= 2)
            {
                WordlistEditor.Load(cmd[1]);
                WordlistEditor.Encoding = Encoding.UTF8;
            }
        }


        private void ShowOrHideWordlist_Click(object sender, RoutedEventArgs e)
        {
            if (this.WordlistTab.SelectedIndex == this.WordlistTab.Items.Count - 1)
                this.WordlistTab.SelectedIndex = 0;
            else
                this.WordlistTab.SelectedIndex++;
        }

        private void StopSpeakingButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.AutoMode)
                controler.StopAuto();
            else if (controler.IsSpeaking)
                controler.StopThis();
        }

        private void StartSpeakingButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.AutoMode)
                controler.StopAuto();
            else if (controler.IsSpeaking)
                controler.StopThis();
            controler.ResetProgress();
            controler.StartAuto();
        }

        private void SpeakNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.WordListSource.Length == 0)
            {
                MessageBox.Show("请先添加词语！");
                return;
            }
            if (controler.NextWordIndex >= controler.WordListSource.Length)
            {
                MessageBox.Show("已经播完了。");
                return;
            }
            controler.SpeakNext();
        }

        private void ResetRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!controler.AutoMode)
            {
                controler.ResetProgress();
            }
            else
            {
                MessageBox.Show(this, "请先停止自动播报！");
            }
        }

        private void SpeakButton_Click(object sender, RoutedEventArgs e)
        {
            if ((controler.NextWordIndex <= controler.WordListSource.Length) & (controler.NextWordIndex > 0))
            {
                controler.SpeakAgain();
            }
            else
            {
                MessageBox.Show(this, "还没报过或已移除报过的词语！");
            }
        }

        private void SpeakPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.NextWordIndex <= controler.WordListSource.Length & controler.NextWordIndex > 1)
            {
                controler.SpeakPrevious();
            }
            else
            {
                MessageBox.Show(this, "已经是第1个了！");
            }
        }

        private void PauseSpeakingButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.IsPausedAutoMode)
                controler.ResumeAuto();
            else
                controler.PauseAuto();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Voice = voiceSettingInfo.BaseVoice?.Name;
            Properties.Settings.Default.PathForImprovedResource = voiceSettingInfo.PathForImprovedResource;
            Properties.Settings.Default.FontInfo = PackedFontInfo.From(WordlistEditor);
            Properties.Settings.Default.Save();
        }

        public class VoiceSettingInfo
            : INotifyPropertyChanged
        {
            private IVoice baseVoice;
            private string pathForImprovedResource;

            public VoiceSettingInfo(Speaker speaker)
            {
                this.PropertyChanged += (sender, e) =>
                {
                    var voice = BaseVoice;
                    if (!string.IsNullOrWhiteSpace(PathForImprovedResource))
                    {
                        voice = new ImprovedVoice(BaseVoice, PathForImprovedResource);
                    }
                    if (voice == null)
                        voice = NullVoice.Instance;
                    speaker.Voice = voice;
                };
            }

            public IVoice BaseVoice
            {
                get { return baseVoice; }
                set
                {
                    baseVoice = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BaseVoice)));
                }
            }

            public string PathForImprovedResource
            {
                get { return pathForImprovedResource; }
                set
                {
                    pathForImprovedResource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathForImprovedResource)));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                DefaultExt = "txt",
                Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WordlistEditor.Load(dialog.FileName);
                WordlistEditor.Encoding = Encoding.UTF8;
            }
        }

        private void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WordlistEditor.Save(dialog.FileName);
            }
        }

        private void OnNewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WordlistEditor.Clear();
        }

        private void PreferenceMenu_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PreferenceWindow();
            dialog.ViewModel.Voices = voices;
            dialog.ViewModel.EditorFontInfo = PackedFontInfo.From(WordlistEditor);
            dialog.ViewModel.PathForImprovedResource = voiceSettingInfo.PathForImprovedResource;
            dialog.ViewModel.DefaultChineseVoiceName = Properties.Settings.Default.DefaultChineseVoiceName;
            dialog.ViewModel.DefaultEnglishVoiceName = Properties.Settings.Default.DefaultEnglishVoiceName;
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                dialog.ViewModel.EditorFontInfo.ApplyTo(WordlistEditor);
                voiceSettingInfo.PathForImprovedResource = dialog.ViewModel.PathForImprovedResource;
                Properties.Settings.Default.DefaultChineseVoiceName = dialog.ViewModel.DefaultChineseVoiceName;
                Properties.Settings.Default.DefaultEnglishVoiceName = dialog.ViewModel.DefaultEnglishVoiceName;
            }
        }

        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("词语数量：" + controler.WordListSource.Length.ToString());
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string fileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                WordlistEditor.Load(fileName);
                WordlistEditor.Encoding = Encoding.UTF8;
            }
        }

        private void SwitchToChineseVoice_Click(object sender, RoutedEventArgs e)
        {
            var voice = 
                voices.FirstOrDefault(x => x.Name == Properties.Settings.Default.DefaultChineseVoiceName);
            if (voice == null)
                voice = voices.FirstOrDefault(x => x.Culture.ThreeLetterWindowsLanguageName == "CHS"); //简体
            if (voice == null)
                voice = voices.FirstOrDefault(x => x.Culture.ThreeLetterWindowsLanguageName == "CHT"); //繁体
            if (voice == null)
                MessageBox.Show("未能找到中文引擎");
            else
                voiceSettingInfo.BaseVoice = voice;
        }

        private void SwitchToEnglishVoice_Click(object sender, RoutedEventArgs e)
        {
            var voice =
                voices.FirstOrDefault(x => x.Name == Properties.Settings.Default.DefaultEnglishVoiceName);
            if (voice == null)
                voice = voices.FirstOrDefault(x => x.Culture.ThreeLetterWindowsLanguageName == "ENG"); //BrE
            if (voice == null)
                voice = voices.FirstOrDefault(x => x.Culture.ThreeLetterWindowsLanguageName == "ENU"); //AmE
            if (voice == null)
                MessageBox.Show("未能找到中文引擎");
            else
                voiceSettingInfo.BaseVoice = voice;
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        private void SaveAudioMenu_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveAudioDialog();
            if(dialog.ShowDialog().GetValueOrDefault(false))
            {
                var progressDialog = new System.Windows.Forms.ProgressDialog
                {
                    Title = "保存音频",
                    Maximum = 10000,
                    AutoClose = false
                };
                progressDialog.SetLine(1, "正在保存音频文件");
                progressDialog.SetLine(2, "请稍后...");
                var worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                worker.DoWork += (senderOfWorker, eOfWorker) =>
                {
                    using (var pcmWriter = new PcmWriter(dialog.ViewModel.CreateEncoder()))
                    {
                        var lyricWriter = dialog.ViewModel.CreateLyricWriter();
                        var speakParam = new SpeakParam() { Rate = speaker.Rate };
                        var complete = controler.SaveAudio(
                            speaker.Voice, 
                            speakParam,
                            pcmWriter,
                            lyricWriter,
                            x => worker.ReportProgress((int)(x * 10000)),
                            () => worker.CancellationPending);
                        lyricWriter?.Flush();
                        if (!complete)
                        {
                            eOfWorker.Cancel = true;
                        }
                    }
                };
                worker.ProgressChanged += (senderOfWorker, eOfWorker) =>
                {
                    progressDialog.Value = (ulong)eOfWorker.ProgressPercentage;
                };
                worker.RunWorkerCompleted += (senderOfWorker, eOfWorker) =>
                {
                    progressDialog.Close();
                    if (eOfWorker.Cancelled)
                    {
                        MessageBox.Show("Cancelled");
                    }
                    else
                    {
                        MessageBox.Show("Done");
                    }
                };
                progressDialog.Canceled += (senderOfDialog, eOfDialog) =>
                {
                    worker.CancelAsync();
                };
                worker.RunWorkerAsync();
                progressDialog.Show();
            }
        }

        private void SpeakSelectionMenu_Click(object sender, RoutedEventArgs e)
        {
            var offest = WordlistEditor.SelectionStart;
            if (offest == -1)
                offest = WordlistEditor.CaretOffset;
            var lineNumber = WordlistEditor.Document.GetLineByOffset(offest).LineNumber;
            controler.Speak(lineNumber - 1);
        }

        private void ViewSelectionInBingDictionaryMenu_Click(object sender, RoutedEventArgs e)
        {
            var selection = WordlistEditor.SelectedText;
            if (string.IsNullOrEmpty(selection))
            {
                var line = WordlistEditor.Document.GetLineByOffset(WordlistEditor.CaretOffset);
                selection = WordlistEditor.Document.GetText(line.Offset, line.Length);
            }
            Process.Start("https://cn.bing.com/dict/search?q=" + Uri.EscapeDataString(selection) + "&qs=n");
        }

        private void StartFromSelectionMenu_Click(object sender, RoutedEventArgs e)
        {
            var offest = WordlistEditor.SelectionStart;
            if (offest == -1)
                offest = WordlistEditor.CaretOffset;
            var lineNumber = WordlistEditor.Document.GetLineByOffset(offest).LineNumber;
            if (controler.AutoMode)
                controler.StopAuto();
            controler.ResetProgress();
            controler.StartAuto(lineNumber - 1);
        }

        private void WordlistEditor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var editor = (TextEditor)sender;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var textArea = editor.TextArea;
                var textView = editor.TextArea.TextView;
                Point pos = e.GetPosition(textView);
                if (pos.Y < 0)
                    pos.Y = 0;
                if (pos.Y > textView.ActualHeight)
                    pos.Y = textView.ActualHeight;
                pos += textView.ScrollOffset;
                if (pos.Y >= textView.DocumentHeight)
                    pos.Y = textView.DocumentHeight - 0.01;
                var line = textView.GetVisualLineFromVisualTop(pos.Y);
                if (line != null)
                {
                    var visualColumn = line.GetVisualColumn(pos, textArea.Selection.EnableVirtualSpace);
                    var offest = line.GetRelativeOffset(visualColumn) + line.FirstDocumentLine.Offset;
                    if (offest < editor.SelectionStart || offest >= editor.SelectionStart + editor.SelectionLength)
                        WordlistEditor.CaretOffset = offest;
                }
            }
        }
    }
}
