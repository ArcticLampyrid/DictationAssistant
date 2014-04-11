Public Class Form1
    Dim WithEvents SpVoice1 As New SpeechLib.SpVoice
    Dim XXGBBCYSY As Integer '下一个播报词语索引
    Dim intYGMS As Integer '已过秒数
    Dim Zdbb As Boolean '（是否）自动播报
    Dim EnglishVoiceGood As Boolean '英文语音引擎语音质量是否良好
    Dim ChineseVoiceGood As Boolean '中文语音引擎语音质量是否良好
    Private Sub OpenButton_Click(sender As Object, e As EventArgs) Handles OpenButton.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            For Each FileName As String In OpenFileDialog1.FileNames
                OpenFile(FileName)
            Next
        End If
    End Sub
    Public Sub OpenFile(FileName As String) '打开词语文件（FileName——文件路径）
        Dim ReadText As New System.IO.StreamReader(FileName, System.Text.Encoding.Default)
        While Not ReadText.EndOfStream
            ListView1.Items.Add(ReadText.ReadLine()).SubItems.Add("0")
        End While
        ReadText.Dispose()
    End Sub

    Private Sub CountButton_Click(sender As Object, e As EventArgs) Handles CountButton.Click
        MessageBox.Show("词语数量：" & ListView1.Items.Count.ToString)
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        ListView1.Items.Clear()
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        For Each SelectedItem As System.Windows.Forms.ListViewItem In ListView1.SelectedItems
            SelectedItem.Remove()
        Next
    End Sub


    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Dim AddString As String
        AddString = InputBox("要添加的词语：", "添加词语")
        If AddString <> "" Then
            ListView1.Items.Add(AddString).SubItems.Add("0")
        End If
    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.SelectedItems.Item(ListView1.SelectedItems.Count - 1).BeginEdit()
        End If
    End Sub

    Private Sub UpButton_Click(sender As Object, e As EventArgs) Handles UpButton.Click
        For Each SelectedItem As System.Windows.Forms.ListViewItem In ListView1.SelectedItems
            Dim SelectedItemIndex As Integer = SelectedItem.Index
            If SelectedItemIndex > 0 Then
                ListView1.Items.Remove(SelectedItem)
                ListView1.Items.Insert(SelectedItemIndex - 1, SelectedItem).EnsureVisible()
            End If
        Next
    End Sub

    Private Sub DownButton_Click(sender As Object, e As EventArgs) Handles DownButton.Click
        For Each SelectedItem As System.Windows.Forms.ListViewItem In ListView1.SelectedItems
            Dim SelectedItemIndex As Integer = SelectedItem.Index
            If SelectedItemIndex < ListView1.Items.Count - 1 Then
                ListView1.Items.Remove(SelectedItem)
                ListView1.Items.Insert(SelectedItemIndex + 1, SelectedItem).EnsureVisible()
            End If
        Next
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Call OpenButton_Click(OpenToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Public Sub SaveFile(FileName As String) '保存词语文件（FileName——文件路径）
        Dim WriterText As New System.IO.StreamWriter(FileName, False, System.Text.Encoding.Default)
        For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.Items
            WriterText.WriteLine(ListItem.Text)
        Next
        WriterText.Dispose()
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            SaveFile(SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListView1.Items.Count = 0 Then
            MessageBox.Show("请先添加词语！")
            Exit Sub
        End If
        If XXGBBCYSY >= ListView1.Items.Count Then
            MessageBox.Show("已经播完了。")
            Exit Sub
        End If
        If Len(DCZQML) = 0 Then
            SpVoice1.Speak(ListView1.Items(XXGBBCYSY).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
        Else
            Dim StreamFileName As String
            StreamFileName = ListView1.Items(XXGBBCYSY).Text
            StreamFileName = StreamFileName.Replace("\", "")
            StreamFileName = StreamFileName.Replace("/", "")
            StreamFileName = StreamFileName.Replace(":", "")
            StreamFileName = StreamFileName.Replace("*", "")
            StreamFileName = StreamFileName.Replace("?", "")
            StreamFileName = StreamFileName.Replace(Chr(34), "")
            StreamFileName = StreamFileName.Replace("<", "")
            StreamFileName = StreamFileName.Replace(">", "")
            StreamFileName = StreamFileName.Replace("|", "")
            StreamFileName = DCZQML & StreamFileName & ".wav"
            If System.IO.File.Exists(StreamFileName) = True Then
                Dim FileStream As New SpeechLib.SpFileStream
                FileStream.Open(StreamFileName)
                SpVoice1.SpeakStream(FileStream, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
            Else
                SpVoice1.Speak(ListView1.Items(XXGBBCYSY).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
            End If
        End If
        Label4.Text = "正在播报第" & (XXGBBCYSY + 1).ToString & "个"
        ListView1.Items(XXGBBCYSY).SubItems.Item(1).Text = (Convert.ToInt32(ListView1.Items(XXGBBCYSY).SubItems.Item(1).Text) + 1).ToString
        Label1.Text = "本词语播报次数：" & ListView1.Items(XXGBBCYSY).SubItems.Item(1).Text
        If 自动翻页ToolStripMenuItem.Checked = True Then
            ListView1.Items(XXGBBCYSY).EnsureVisible()
        End If
        If 字词跟随ToolStripMenuItem.Checked = True Then
            For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.SelectedItems
                ListItem.Selected = False
            Next
            ListView1.Items(XXGBBCYSY).Selected = True
        End If
        XXGBBCYSY = XXGBBCYSY + 1
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim EnglishGrade As Integer, ChineseGrade As Integer '声明用于记录语音引擎质量等级的变量
        XXGBBCYSY = 0
        Zdbb = False
        For Each Temp_Voice As SpeechLib.SpObjectToken In SpVoice1.GetVoices
            Dim Temp_VoiceName As String
            Temp_VoiceName = Temp_Voice.GetDescription
            If GetVoiceLanguage(Temp_Voice) = VoiceLanguage.英文 Then
                ComboBox1.Items.Add("（英文）" & Temp_VoiceName)
                If (EnglishGrade = 0) Or (EnglishGrade > GetEnglishGrade(Temp_VoiceName) And GetEnglishGrade(Temp_VoiceName) <> 0) Then
                    Button11.Tag = (ComboBox1.Items.Count - 1)
                    EnglishGrade = GetEnglishGrade(Temp_VoiceName)
                End If
            ElseIf GetVoiceLanguage(Temp_Voice) = VoiceLanguage.中文 Then
                ComboBox1.Items.Add("（中文）" & Temp_VoiceName)
                If (ChineseGrade = 0) Or (ChineseGrade > GetChineseGrade(Temp_VoiceName) And GetChineseGrade(Temp_VoiceName) <> 0) Then
                    Button10.Tag = (ComboBox1.Items.Count - 1)
                    ChineseGrade = GetChineseGrade(Temp_VoiceName)
                End If
            Else
                ComboBox1.Items.Add(Temp_VoiceName)
            End If
        Next
        EnglishVoiceGood = EnglishGrade > 0
        ChineseVoiceGood = ChineseGrade > 0 And EnglishGrade < 5
        DCZQML = GetSetting("自动默写", "TTS引擎", "单词增强", "")
        TrackBar1.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "音量", "100"))
        SpVoice1.Volume = TrackBar1.Value
        TrackBar2.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "语速", "0"))
        SpVoice1.Rate = TrackBar2.Value
        ComboBox1.SelectedIndex = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "引擎", "0"))
        自动翻页ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "自动翻页", System.Boolean.TrueString))
        字词跟随ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "字词跟随", System.Boolean.TrueString))
        Dim ListViewFontName As String = GetSetting("自动默写", "字体", "名称", ListView1.Font.Name)
        Dim ListViewFontSize As Single = Convert.ToSingle(GetSetting("自动默写", "字体", "大小", ListView1.Font.Size.ToString))
        Dim ListViewFontGdiVertical As Boolean = Convert.ToBoolean(GetSetting("自动默写", "字体", "垂直", ListView1.Font.GdiVerticalFont.ToString))
        Dim ListViewFontFontGdiCharSet As Byte = Convert.ToByte(GetSetting("自动默写", "字体", "字符集", ListView1.Font.GdiCharSet.ToString))
        Dim ListViewFontStyle As FontStyle = GetSetting("自动默写", "字体", "样式", Convert.ToInt32(ListView1.Font.Style).ToString)
        ListView1.Font = New Font(ListViewFontName, ListViewFontSize, ListViewFontStyle, ListView1.Font.Unit, ListViewFontFontGdiCharSet, ListViewFontGdiVertical)
        ListView1.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(GetSetting("自动默写", "字体", "颜色", ListView1.ForeColor.ToArgb.ToString)))
        For Each CommandLine As String In My.Application.CommandLineArgs
            If System.IO.File.Exists(CommandLine) = True Then
                OpenFile(CommandLine)
            End If
        Next
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If (XXGBBCYSY <= ListView1.Items.Count) And (XXGBBCYSY > 0) Then
            If Len(DCZQML) = 0 Then
                SpVoice1.Speak(ListView1.Items(XXGBBCYSY - 1).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
            Else
                Dim StreamFileName As String
                StreamFileName = ListView1.Items(XXGBBCYSY - 1).Text
                StreamFileName = StreamFileName.Replace("\", "")
                StreamFileName = StreamFileName.Replace("/", "")
                StreamFileName = StreamFileName.Replace(":", "")
                StreamFileName = StreamFileName.Replace("*", "")
                StreamFileName = StreamFileName.Replace("?", "")
                StreamFileName = StreamFileName.Replace(Chr(34), "")
                StreamFileName = StreamFileName.Replace("<", "")
                StreamFileName = StreamFileName.Replace(">", "")
                StreamFileName = StreamFileName.Replace("|", "")
                StreamFileName = DCZQML & StreamFileName & ".wav"
                If System.IO.File.Exists(StreamFileName) = True Then
                    Dim FileStream As New SpeechLib.SpFileStream
                    FileStream.Open(StreamFileName)
                    SpVoice1.SpeakStream(FileStream, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
                Else
                    SpVoice1.Speak(ListView1.Items(XXGBBCYSY - 1).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
                End If
            End If
            ListView1.Items(XXGBBCYSY - 1).SubItems.Item(1).Text = (Convert.ToInt32(ListView1.Items(XXGBBCYSY - 1).SubItems.Item(1).Text) + 1).ToString
            Label4.Text = "正在播报第" & (XXGBBCYSY).ToString & "个"
            Label1.Text = "本词语播报次数：" & ListView1.Items(XXGBBCYSY - 1).SubItems.Item(1).Text
            If 自动翻页ToolStripMenuItem.Checked = True Then
                ListView1.Items(XXGBBCYSY - 1).EnsureVisible()
            End If
            If 字词跟随ToolStripMenuItem.Checked = True Then
                For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.SelectedItems
                    ListItem.Selected = False
                Next
                ListView1.Items(XXGBBCYSY - 1).Selected = True
            End If
        Else
            MessageBox.Show("还没报过或已删除报过的词语！")
        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If XXGBBCYSY <= ListView1.Items.Count And XXGBBCYSY > 1 Then
            XXGBBCYSY = XXGBBCYSY - 2
            Button4_Click(Button7, System.EventArgs.Empty)
        Else
            MessageBox.Show("已经是第1个了！")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Zdbb = False Then
            XXGBBCYSY = 0
            For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.Items
                ListItem.SubItems.Item(1).Text = "0"
            Next
        Else
            MessageBox.Show("请先停止自动播报！")
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim BXYG As Boolean '报下一个
        intYGMS = intYGMS + 1
        If XXGBBCYSY = 0 Then
            BXYG = True
        ElseIf XXGBBCYSY <= ListView1.Items.Count Then
            If Convert.ToInt32(ListView1.Items(XXGBBCYSY - 1).SubItems.Item(1).Text) >= Convert.ToInt32(TextBox2.Text) Then
                BXYG = True
            Else
                BXYG = False
            End If
        Else
            zdbbyfc()
            Exit Sub
        End If
        If Convert.ToInt32(TextBox1.Text) - intYGMS > 0 Then
            If BXYG = True Then
                Label4.Text = (Convert.ToInt32(TextBox1.Text) - intYGMS).ToString & "秒后自动播报第" & (XXGBBCYSY + 1).ToString & "个"
            Else
                Label4.Text = (Convert.ToInt32(TextBox1.Text) - intYGMS).ToString & "秒后自动播报第" & XXGBBCYSY.ToString & "个"
            End If
        End If
        If intYGMS >= Convert.ToInt32(TextBox1.Text) Then
            If BXYG = True Then
                If XXGBBCYSY < ListView1.Items.Count Then
                    Button4_Click(Timer1, System.EventArgs.Empty)
                Else
                    zdbbyfc()
                    Exit Sub
                End If
            Else
                Button8_Click(Timer1, System.EventArgs.Empty)
            End If
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ListView1.Items.Count = 0 Then
            MessageBox.Show("请先添加词语！")
            Exit Sub
        End If
        Button1.Enabled = False
        Button2.Enabled = True
        intYGMS = 0
        Timer1.Enabled = True
        Button5_Click(Button1, System.EventArgs.Empty)
        Zdbb = True
        Button4_Click(Button1, System.EventArgs.Empty)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Zdbb = False
        Button1.Enabled = True
        Button3.Enabled = False
        Button2.Enabled = False
        Timer1.Enabled = False
        If SpVoice1.Status.RunningState <> SpeechLib.SpeechRunState.SRSEDone Then
            SpVoice1.Speak(String.Empty, SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak)
            Button4.Enabled = True
            Button8.Enabled = True
            Button7.Enabled = True
            读选定词语ToolStripMenuItem.Enabled = True
        End If
        Label4.Text = "等待播报"
        MessageBox.Show("自动播报已结束！")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button3.Enabled = False
        Timer1.Enabled = False
        Label4.Text = "自动播报已暂停"
        Button8.Focus()
    End Sub

    Private Sub SpVoice1_EndStream(StreamNumber As Integer, StreamPosition As Object) Handles SpVoice1.EndStream
        Dim intBBCS As Integer
        If XXGBBCYSY <= ListView1.Items.Count Then
            intBBCS = Convert.ToInt32(ListView1.Items(XXGBBCYSY - 1).SubItems.Item(1).Text)
        Else
            zdbbyfc()
            Exit Sub
        End If

        Button4.Enabled = True
        Button8.Enabled = True
        If Zdbb = True Then Button3.Enabled = True
        Button7.Enabled = True
        读选定词语ToolStripMenuItem.Enabled = True
        If Zdbb = True Then
            If intBBCS >= Convert.ToInt32(TextBox2.Text) And XXGBBCYSY < ListView1.Items.Count Then
                intYGMS = 0
                If TextBox1.Text = "0" Then
                    Timer1_Tick(SpVoice1, System.EventArgs.Empty)
                Else
                    Label4.Text = TextBox1.Text & "秒后自动播报第" & (XXGBBCYSY + 1).ToString & "个"
                    Timer1.Enabled = True
                End If
            ElseIf intBBCS < Convert.ToInt32(TextBox2.Text) And XXGBBCYSY <= ListView1.Items.Count Then
                intYGMS = 0
                If TextBox1.Text = "0" Then
                    Timer1_Tick(SpVoice1, System.EventArgs.Empty)
                Else
                    Label4.Text = TextBox1.Text & "秒后自动播报第" & XXGBBCYSY.ToString & "个"
                    Timer1.Enabled = True
                End If
            Else
                zdbbyfc()
            End If
        Else
            Label4.Text = "等待播报"
        End If

    End Sub

    Private Sub SpVoice1_StartStream(StreamNumber As Integer, StreamPosition As Object) Handles SpVoice1.StartStream
        Button4.Enabled = False
        Button8.Enabled = False
        Button7.Enabled = False
        Button3.Enabled = False
        读选定词语ToolStripMenuItem.Enabled = False
        Timer1.Enabled = False
    End Sub

    Private Sub 保存SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存SToolStripMenuItem.Click
        Call SaveButton_Click(保存SToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 退出EToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出EToolStripMenuItem.Click
        End
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Button9.Text = "隐藏词语列表" Then
            ListView1.Visible = False
            OpenButton.Visible = False
            SaveButton.Visible = False
            UpButton.Visible = False
            DownButton.Visible = False
            AddButton.Visible = False
            EditButton.Visible = False
            DeleteButton.Visible = False
            ClearButton.Visible = False
            CountButton.Visible = False
            Label5.Visible = True
            Button9.Text = "显示词语列表"
        Else
            ListView1.Visible = True
            OpenButton.Visible = True
            SaveButton.Visible = True
            UpButton.Visible = True
            DownButton.Visible = True
            AddButton.Visible = True
            EditButton.Visible = True
            DeleteButton.Visible = True
            ClearButton.Visible = True
            CountButton.Visible = True
            Label5.Visible = False
            Button9.Text = "隐藏词语列表"
        End If
    End Sub


    Private Sub GroupBox3_Resize(sender As Object, e As EventArgs) Handles GroupBox3.Resize
        Dim Temp As Integer

        Temp = Convert.ToInt32((GroupBox3.Height - 47) / 5)
        Button1.Height = Temp
        Button2.Height = Temp
        Button3.Top = Button1.Top + Button1.Height + 6
        Button3.Height = Temp * 2
        Button4.Height = Temp
        Button5.Height = Temp
        Button4.Top = Button3.Top + Button3.Height + 6
        Button5.Top = Button3.Top + Button3.Height + 6
        Button7.Height = Temp
        Button8.Height = Temp
        Button7.Top = Button4.Top + Button4.Height + 6
        Button8.Top = Button4.Top + Button4.Height + 6
    End Sub

    Private Sub 上移ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 上移ToolStripMenuItem.Click
        UpButton_Click(上移ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 下移ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 下移ToolStripMenuItem.Click
        DownButton_Click(下移ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 修改ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 修改ToolStripMenuItem.Click
        EditButton_Click(修改ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 删除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 删除ToolStripMenuItem.Click
        DeleteButton_Click(删除ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 反向选择ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 反向选择ToolStripMenuItem.Click
        反向选择ToolStripMenuItem1_Click(反向选择ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 显示隐藏词语列表ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 显示隐藏词语列表ToolStripMenuItem.Click
        Call Button9_Click(显示隐藏词语列表ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 添加词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 添加词语ToolStripMenuItem.Click
        Call AddButton_Click(添加词语ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 删除选中词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 删除选中词语ToolStripMenuItem.Click
        Call DeleteButton_Click(删除选中词语ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 清空词语列表ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清空词语列表ToolStripMenuItem.Click
        Call ClearButton_Click(清空词语列表ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 修改选中词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 修改选中词语ToolStripMenuItem.Click
        Call EditButton_Click(修改选中词语ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 向上移动ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 向上移动ToolStripMenuItem.Click
        Call UpButton_Click(向上移动ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 向下移动ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 向下移动ToolStripMenuItem.Click
        Call DownButton_Click(向下移动ToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 反向选择ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 反向选择ToolStripMenuItem1.Click
        For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.Items
            ListItem.Selected = Not ListItem.Selected
        Next
    End Sub


    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button10.Tag Is Nothing Then
            MessageBox.Show("未发现中文语音引擎!")
            Exit Sub
        End If
        If ChineseVoiceGood = False Then
            MessageBox.Show("建议安装更好的中文语音引擎")
        End If
        ComboBox1.SelectedIndex = Convert.ToInt32(Button10.Tag)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Button11.Tag Is Nothing Then
            MessageBox.Show("未发现英文语音引擎!")
            Exit Sub
        End If
        If EnglishVoiceGood = False Then
            MessageBox.Show("建议安装更好的英文语音引擎")
        End If
        ComboBox1.SelectedIndex = Convert.ToInt32(Button11.Tag)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SpVoice1.Voice = SpVoice1.GetVoices.Item(ComboBox1.SelectedIndex)
        SaveSetting("自动默写", "TTS引擎", "引擎", ComboBox1.SelectedIndex.ToString)
    End Sub

    Private Sub 设置单词增强ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 设置单词增强ToolStripMenuItem.Click
        Form2.ShowDialog()
    End Sub

    Private Sub 自动翻页ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 自动翻页ToolStripMenuItem.Click
        自动翻页ToolStripMenuItem.Checked = Not 自动翻页ToolStripMenuItem.Checked
        SaveSetting("自动默写", "选项", "自动翻页", 自动翻页ToolStripMenuItem.Checked.ToString)
        If 自动翻页ToolStripMenuItem.Checked = False Then
            字词跟随ToolStripMenuItem.Checked = False
            SaveSetting("自动默写", "选项", "字词跟随", 字词跟随ToolStripMenuItem.Checked.ToString)
        End If
    End Sub

    Private Sub 字词跟随ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 字词跟随ToolStripMenuItem.Click
        字词跟随ToolStripMenuItem.Checked = Not 字词跟随ToolStripMenuItem.Checked
        SaveSetting("自动默写", "选项", "字词跟随", 字词跟随ToolStripMenuItem.Checked.ToString)
        If 自动翻页ToolStripMenuItem.Checked = False Then
            自动翻页ToolStripMenuItem.Checked = True
            SaveSetting("自动默写", "选项", "自动翻页", 自动翻页ToolStripMenuItem.Checked.ToString)
        End If
    End Sub

    Private Sub 关于AToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 关于AToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        SpVoice1.Volume = TrackBar1.Value
        SaveSetting("自动默写", "TTS引擎", "音量", SpVoice1.Volume.ToString)
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        SpVoice1.Rate = TrackBar2.Value
        SaveSetting("自动默写", "TTS引擎", "语速", SpVoice1.Rate.ToString)
    End Sub


    Private Sub 全选ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem1.Click
        全选ToolStripMenuItem_Click(全选ToolStripMenuItem1, System.EventArgs.Empty)
    End Sub

    Private Sub 全选ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem.Click
        For Each ListItem As System.Windows.Forms.ListViewItem In ListView1.Items
            ListItem.Selected = True
        Next
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub



    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            TextBox1.Text = "0"
            TextBox1.Select(0, 1)
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text = "" Then
            TextBox2.Text = "1"
            TextBox2.Select(0, 1)
        End If
    End Sub

    Private Sub 随机排序ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机排序ToolStripMenuItem.Click
        For EndIndex As Integer = ListView1.Items.Count To 2 Step -1
            Dim rand = New System.Random()
            Dim ListItem As System.Windows.Forms.ListViewItem
            Dim xxczdsyh As Integer
            xxczdsyh = rand.Next(0, EndIndex)
            ListItem = ListView1.Items.Item(xxczdsyh)
            ListView1.Items.Item(xxczdsyh).Remove()
            ListView1.Items.Add(ListItem)
        Next
    End Sub

    Private Sub 随机选词ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机选词ToolStripMenuItem.Click
        Form3.ShowDialog()
    End Sub

    Private Sub FontDialog1_Apply(sender As Object, e As EventArgs) Handles FontDialog1.Apply
        ListView1.Font = FontDialog1.Font
        ListView1.ForeColor = FontDialog1.Color
    End Sub

    Private Sub 字体FToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 字体FToolStripMenuItem.Click
        Dim ListViewFont As Font
        Dim ListViewForeColor As System.Drawing.Color
        Dim FontReturnValue As System.Windows.Forms.DialogResult
        FontDialog1.Font = ListView1.Font
        FontDialog1.Color = ListView1.ForeColor
        ListViewFont = ListView1.Font
        ListViewForeColor = ListView1.ForeColor
        FontReturnValue = FontDialog1.ShowDialog
        If FontReturnValue = Windows.Forms.DialogResult.OK Then
            ListView1.Font = FontDialog1.Font
            ListView1.ForeColor = FontDialog1.Color
            SaveSetting("自动默写", "字体", "名称", ListView1.Font.Name)
            SaveSetting("自动默写", "字体", "大小", ListView1.Font.Size.ToString)
            SaveSetting("自动默写", "字体", "样式", Convert.ToInt32(ListView1.Font.Style).ToString)
            SaveSetting("自动默写", "字体", "垂直", ListView1.Font.GdiVerticalFont.ToString)
            SaveSetting("自动默写", "字体", "字符集", ListView1.Font.GdiCharSet.ToString)
            SaveSetting("自动默写", "字体", "颜色", ListView1.ForeColor.ToArgb.ToString)
        ElseIf FontReturnValue = Windows.Forms.DialogResult.Cancel Then
            ListView1.Font = ListViewFont
            ListView1.ForeColor = ListViewForeColor
        End If
    End Sub
    Sub zdbbyfc() '自动播报已完成
        Zdbb = False
        Button1.Enabled = True
        Button3.Enabled = False
        Button2.Enabled = False
        Button4.Enabled = True
        Button8.Enabled = True
        Button7.Enabled = True
        Timer1.Enabled = False
        读选定词语ToolStripMenuItem.Enabled = True
        Label4.Text = "等待播报"
        MessageBox.Show("自动播报已完成！")
    End Sub

    Private Sub 读选定词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 读选定词语ToolStripMenuItem.Click
        Dim SelectedIndex As Integer
        If ListView1.SelectedIndices.Count > 0 Then
            SelectedIndex = ListView1.SelectedIndices.Item(0)
            XXGBBCYSY = SelectedIndex
            Button4_Click(读选定词语ToolStripMenuItem, EventArgs.Empty)
        End If
    End Sub

End Class
