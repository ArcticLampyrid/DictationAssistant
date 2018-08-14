<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SaveAudioDialog
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChannelComboBox = New System.Windows.Forms.ComboBox()
        Me.SampleFormatComboBox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FreqComboBox = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TargetFileTextBox = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OutputFormatComboBox = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "声道"
        '
        'ChannelComboBox
        '
        Me.ChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ChannelComboBox.FormattingEnabled = True
        Me.ChannelComboBox.Items.AddRange(New Object() {"单声道", "立体声"})
        Me.ChannelComboBox.Location = New System.Drawing.Point(71, 6)
        Me.ChannelComboBox.Name = "ChannelComboBox"
        Me.ChannelComboBox.Size = New System.Drawing.Size(225, 20)
        Me.ChannelComboBox.TabIndex = 1
        '
        'SampleFormatComboBox
        '
        Me.SampleFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SampleFormatComboBox.FormattingEnabled = True
        Me.SampleFormatComboBox.Items.AddRange(New Object() {"Unsigned 8bit", "Signed 16bit"})
        Me.SampleFormatComboBox.Location = New System.Drawing.Point(71, 32)
        Me.SampleFormatComboBox.Name = "SampleFormatComboBox"
        Me.SampleFormatComboBox.Size = New System.Drawing.Size(225, 20)
        Me.SampleFormatComboBox.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "位宽"
        '
        'FreqComboBox
        '
        Me.FreqComboBox.FormattingEnabled = True
        Me.FreqComboBox.Items.AddRange(New Object() {"6000", "7333", "8000", "11025", "16000", "22050", "24000", "32000", "44100", "48000"})
        Me.FreqComboBox.Location = New System.Drawing.Point(71, 58)
        Me.FreqComboBox.Name = "FreqComboBox"
        Me.FreqComboBox.Size = New System.Drawing.Size(225, 20)
        Me.FreqComboBox.TabIndex = 5
        Me.FreqComboBox.Text = "44100"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "采样率"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "目标文件"
        '
        'TargetFileTextBox
        '
        Me.TargetFileTextBox.Location = New System.Drawing.Point(71, 112)
        Me.TargetFileTextBox.Name = "TargetFileTextBox"
        Me.TargetFileTextBox.Size = New System.Drawing.Size(164, 21)
        Me.TargetFileTextBox.TabIndex = 7
        Me.TargetFileTextBox.Text = "D:\Test.mp3"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(241, 110)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(55, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "浏览"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(241, 139)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(55, 23)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "保存"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.DefaultExt = "wav"
        Me.SaveFileDialog1.Filter = "WAV音频文件(*.wav)|*.wav|MP3音频文件(*.mp3)|*.mp3|Opus音频文件(*.opus)|*.opus|所有文件(*.*)|*.*"
        '
        'OutputFormatComboBox
        '
        Me.OutputFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.OutputFormatComboBox.FormattingEnabled = True
        Me.OutputFormatComboBox.Items.AddRange(New Object() {"【原始】WAV", "【经典】MP3", "【未来】Opus"})
        Me.OutputFormatComboBox.Location = New System.Drawing.Point(71, 84)
        Me.OutputFormatComboBox.Name = "OutputFormatComboBox"
        Me.OutputFormatComboBox.Size = New System.Drawing.Size(225, 20)
        Me.OutputFormatComboBox.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 87)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "输出格式"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        Me.Timer1.Tag = ""
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(71, 144)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(164, 15)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "准备就绪"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(12, 143)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(42, 16)
        Me.CheckBox1.TabIndex = 13
        Me.CheckBox1.Text = "LRC"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'SaveAudioDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 168)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.OutputFormatComboBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TargetFileTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.FreqComboBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.SampleFormatComboBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ChannelComboBox)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "SaveAudioDialog"
        Me.Text = "SaveAudioDialog"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ChannelComboBox As ComboBox
    Friend WithEvents SampleFormatComboBox As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents FreqComboBox As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TargetFileTextBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OutputFormatComboBox As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label6 As Label
    Friend WithEvents CheckBox1 As CheckBox
End Class
