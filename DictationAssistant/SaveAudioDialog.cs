using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.IO;
using System.Threading;

namespace DictationAssistant
{
    public partial class SaveAudioDialog
    {
        private SpeakControler controler;

        public SaveAudioDialog(SpeakControler controler)
        {
            InitializeComponent();
            this.controler = controler ?? throw new ArgumentNullException(nameof(controler));
        }

        private delegate void ActionWithoutArgs();
        private void Button2_Click(object sender, EventArgs e)
        {
            byte channels = ChannelComboBox.SelectedIndex == 0 ? System.Convert.ToByte(1) : System.Convert.ToByte(2);
            PcmSampleFormat sampleFormat = SampleFormatComboBox.SelectedIndex == 0 ? PcmSampleFormat.U8 : PcmSampleFormat.S16;

            int freq;
            try
            {
                freq = Convert.ToInt32(FreqComboBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "采样率必须为纯数字，且不包括Hz等单位符号", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            PcmFormatInfo pcmInfo = new PcmFormatInfo(freq, channels, sampleFormat);

            string targetFile = TargetFileTextBox.Text;
            int outputFormat = OutputFormatComboBox.SelectedIndex;
            var wordListSource = controler.WordListSource;
            var engine = ((Speaker)controler.Speaker).Engine;
            var rate = ((Speaker)controler.Speaker).Rate;
            var speakParam = new SpeakParam() { Rate = rate };
            var waitingTimeCalculator = controler.CurrentWaitingTimeCalculator;
            int timesPerWord = controler.AutoMode_TimesPerWord;
            bool saveLrc = CheckBox1.Checked;
            void writePcmData(PcmStreamWithInfo pcmStream)
            {
                LyricWriter lrcWriter = new LyricWriter();
                var lrcFile = Path.ChangeExtension(targetFile, "lrc");
                long byteOffest = 0;
                using (var pcmWriterObject = new PcmWriter(pcmStream, true))
                {
                    for (int iWord = 0; iWord < wordListSource.Length; iWord++)
                    {
                        string word = wordListSource[iWord];
                        for (var i = 1; i <= timesPerWord; i++)
                        {
                            lrcWriter.Append(word, pcmWriterObject.BytesToMilliseconds(byteOffest));
                            using (var speech = engine.Speak(word, speakParam))
                            {
                                byteOffest += pcmWriterObject.Write(speech);
                            }
                            byteOffest += pcmWriterObject.WriteDelay(waitingTimeCalculator.CalculateWaitingTime(word) * 1000);
                        }
                        lrcWriter.Append(" ", pcmWriterObject.BytesToMilliseconds(byteOffest) - 10);
                    }
                    lrcWriter.Append("本文件由 自动默写 程序自动生成", pcmWriterObject.BytesToMilliseconds(byteOffest));
                }
                if (saveLrc)
                {
                    using (var lrcFileStream = File.Open(lrcFile, FileMode.Create))
                    {
                        lrcWriter.WriteTo(lrcFileStream);
                    }
                }
            }
            this.Enabled = false;

            Timer1.Enabled = true;
            Timer1.Tag = 0;
            Label6.Text = "已用时0秒";
            Thread t = new Thread(() =>
            {
                try
                {
                    if (outputFormat == 0)
                    {
                        using (var pcmStream = new PcmStreamWithInfo(File.Open(targetFile, FileMode.Create), pcmInfo))
                        {
                            PcmAudioHelper.WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, 0);
                            var headerSize = pcmStream.PcmStream.Length;
                            writePcmData(pcmStream);
                            pcmStream.PcmStream.Position = 0;
                            PcmAudioHelper.WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, pcmStream.PcmStream.Length - headerSize);
                        }
                    }
                    else
                    {
                        var EncoderFileName = "";
                        var EncoderArguments = "";
                        switch (outputFormat)
                        {
                            case 1:
                                {
                                    EncoderFileName = "lame.exe";
                                    EncoderArguments = $"--ta 自动默写 -V0 --ignorelength --quiet - \"{targetFile}\"";
                                    break;
                                }

                            case 2:
                                {
                                    EncoderFileName = "opusenc.exe";
                                    EncoderArguments = $"--artist 自动默写 --ignorelength --quiet - \"{targetFile}\"";
                                    break;
                                }

                            default:
                                {
                                    throw new Exception("Something impossible happened.");
                                }
                        }
                        using (Process ProcessObject = new Process())
                        {
                            ProcessObject.StartInfo.FileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + EncoderFileName;
                            ProcessObject.StartInfo.Arguments = EncoderArguments;
                            ProcessObject.StartInfo.UseShellExecute = false;
                            ProcessObject.StartInfo.CreateNoWindow = true;
                            ProcessObject.StartInfo.RedirectStandardInput = true;
                            ProcessObject.Start();
                            using (var pcmStream = new PcmStreamWithInfo(ProcessObject.StandardInput.BaseStream, pcmInfo))
                            {
                                PcmAudioHelper.WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, 0);
                                writePcmData(pcmStream);
                            }
                            ProcessObject.WaitForExit();
                            if (ProcessObject.ExitCode != 0)
                                throw new Exception($"编码错误：{ProcessObject.ExitCode}，编码器：{ProcessObject.StartInfo.FileName}，参数：{ProcessObject.StartInfo.Arguments}");
                        }
                    }

                    
                    this.Invoke(new ActionWithoutArgs(() =>
                    {
                        Timer1.Enabled = false;
                        this.Enabled = true;
                        MessageBox.Show(this, "Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch (Exception ex)
                {
                    Timer1.Enabled = false;
                    this.Invoke(new ActionWithoutArgs(() =>
                    {
                        Timer1.Enabled = false;
                        this.Enabled = true;
                        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
            t.Start();
        }

        private void SaveAudioDialog_Load(object sender, EventArgs e)
        {
            ChannelComboBox.SelectedIndex = 1;
            SampleFormatComboBox.SelectedIndex = 1;
            OutputFormatComboBox.SelectedIndex = 1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            switch (OutputFormatComboBox.SelectedIndex)
            {
                case 0:
                    {
                        SaveFileDialog1.FilterIndex = 1;
                        break;
                    }

                case 1:
                    {
                        SaveFileDialog1.FilterIndex = 2;
                        break;
                    }

                case 2:
                    {
                        SaveFileDialog1.FilterIndex = 3;
                        break;
                    }
            }
            SaveFileDialog1.FileName = TargetFileTextBox.Text;
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (SaveFileDialog1.FilterIndex)
                {
                    case 1:
                        {
                            OutputFormatComboBox.SelectedIndex = 0;
                            break;
                        }

                    case 2:
                        {
                            OutputFormatComboBox.SelectedIndex = 1;
                            break;
                        }

                    case 3:
                        {
                            OutputFormatComboBox.SelectedIndex = 2;
                            break;
                        }
                }
                TargetFileTextBox.Text = SaveFileDialog1.FileName;
            }
        }

        private void OutputFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (OutputFormatComboBox.SelectedIndex)
            {
                case 0:
                    {
                        TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".wav");
                        break;
                    }

                case 1:
                    {
                        TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".mp3");
                        break;
                    }

                case 2:
                    {
                        TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".opus");
                        break;
                    }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var second = System.Convert.ToInt32(Timer1.Tag);
            second += 1;
            Timer1.Tag = second;
            Label6.Text = $"已用时{second}秒";
        }
    }
}
