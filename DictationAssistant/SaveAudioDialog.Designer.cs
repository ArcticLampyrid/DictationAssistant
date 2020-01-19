using System.Windows.Forms;

namespace DictationAssistant
{
    partial class SaveAudioDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Label1 = new System.Windows.Forms.Label();
            this.ChannelComboBox = new System.Windows.Forms.ComboBox();
            this.SampleFormatComboBox = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.FreqComboBox = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.TargetFileTextBox = new System.Windows.Forms.TextBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.OutputFormatComboBox = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label6 = new System.Windows.Forms.Label();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(29, 12);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "声道";
            // 
            // ChannelComboBox
            // 
            this.ChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelComboBox.FormattingEnabled = true;
            this.ChannelComboBox.Items.AddRange(new object[] {
            "单声道",
            "立体声"});
            this.ChannelComboBox.Location = new System.Drawing.Point(71, 6);
            this.ChannelComboBox.Name = "ChannelComboBox";
            this.ChannelComboBox.Size = new System.Drawing.Size(225, 20);
            this.ChannelComboBox.TabIndex = 1;
            // 
            // SampleFormatComboBox
            // 
            this.SampleFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SampleFormatComboBox.FormattingEnabled = true;
            this.SampleFormatComboBox.Items.AddRange(new object[] {
            "Unsigned 8bit",
            "Signed 16bit"});
            this.SampleFormatComboBox.Location = new System.Drawing.Point(71, 32);
            this.SampleFormatComboBox.Name = "SampleFormatComboBox";
            this.SampleFormatComboBox.Size = new System.Drawing.Size(225, 20);
            this.SampleFormatComboBox.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 35);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(29, 12);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "位宽";
            // 
            // FreqComboBox
            // 
            this.FreqComboBox.FormattingEnabled = true;
            this.FreqComboBox.Items.AddRange(new object[] {
            "6000",
            "7333",
            "8000",
            "11025",
            "16000",
            "22050",
            "24000",
            "32000",
            "44100",
            "48000"});
            this.FreqComboBox.Location = new System.Drawing.Point(71, 58);
            this.FreqComboBox.Name = "FreqComboBox";
            this.FreqComboBox.Size = new System.Drawing.Size(225, 20);
            this.FreqComboBox.TabIndex = 5;
            this.FreqComboBox.Text = "44100";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(12, 61);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(41, 12);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "采样率";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(12, 115);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(53, 12);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "目标文件";
            // 
            // TargetFileTextBox
            // 
            this.TargetFileTextBox.Location = new System.Drawing.Point(71, 112);
            this.TargetFileTextBox.Name = "TargetFileTextBox";
            this.TargetFileTextBox.Size = new System.Drawing.Size(164, 21);
            this.TargetFileTextBox.TabIndex = 7;
            this.TargetFileTextBox.Text = "D:\\Test.mp3";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(241, 110);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(55, 23);
            this.Button1.TabIndex = 8;
            this.Button1.Text = "浏览";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(241, 139);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(55, 23);
            this.Button2.TabIndex = 9;
            this.Button2.Text = "保存";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // SaveFileDialog1
            // 
            this.SaveFileDialog1.DefaultExt = "wav";
            this.SaveFileDialog1.Filter = "WAV音频文件(*.wav)|*.wav|MP3音频文件(*.mp3)|*.mp3|Opus音频文件(*.opus)|*.opus|所有文件(*.*)|*.*";
            // 
            // OutputFormatComboBox
            // 
            this.OutputFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OutputFormatComboBox.FormattingEnabled = true;
            this.OutputFormatComboBox.Items.AddRange(new object[] {
            "【原始】WAV",
            "【经典】MP3",
            "【未来】Opus"});
            this.OutputFormatComboBox.Location = new System.Drawing.Point(71, 84);
            this.OutputFormatComboBox.Name = "OutputFormatComboBox";
            this.OutputFormatComboBox.Size = new System.Drawing.Size(225, 20);
            this.OutputFormatComboBox.TabIndex = 11;
            this.OutputFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.OutputFormatComboBox_SelectedIndexChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(12, 87);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(53, 12);
            this.Label5.TabIndex = 10;
            this.Label5.Text = "输出格式";
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tag = "";
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Label6
            // 
            this.Label6.Location = new System.Drawing.Point(71, 144);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(164, 15);
            this.Label6.TabIndex = 12;
            this.Label6.Text = "准备就绪";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Checked = true;
            this.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox1.Location = new System.Drawing.Point(12, 143);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(42, 16);
            this.CheckBox1.TabIndex = 13;
            this.CheckBox1.Text = "LRC";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // SaveAudioDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 168);
            this.Controls.Add(this.CheckBox1);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.OutputFormatComboBox);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.TargetFileTextBox);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.FreqComboBox);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.SampleFormatComboBox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.ChannelComboBox);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SaveAudioDialog";
            this.Text = "SaveAudioDialog";
            this.Load += new System.EventHandler(this.SaveAudioDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Label Label1;

        private ComboBox ChannelComboBox;

        private ComboBox SampleFormatComboBox;

        private Label Label2;

        private ComboBox FreqComboBox;

        private Label Label3;

        private Label Label4;

        private TextBox TargetFileTextBox;

        private Button Button1;

        private Button Button2;

        private SaveFileDialog SaveFileDialog1;

        private ComboBox OutputFormatComboBox;

        private Label Label5;

        private Timer Timer1;

        private Label Label6;

        private CheckBox CheckBox1;
    }
}
