Imports System.IO

Public Class Form1
    Dim SpeakEngines As New List(Of ISpeakEngine)
    Friend WithEvents BoBao As New SpeakControler

    ''' <summary>
    ''' 打开多个词语文件
    ''' </summary>
    ''' <param name="Files">词语文件列表（Ansi编码）</param>
    ''' <remarks>不会在列表中移除原词语</remarks>
    Public Sub OpenFiles(Files As String())
        WordListView.BeginUpdate()

        For Each FileName As String In Files
            Using ReadText As New StreamReader(FileName, System.Text.Encoding.Default)
                While Not ReadText.EndOfStream
                    Dim CiYu As String = ReadText.ReadLine()
                    If CiYu.Trim() <> "" Then
                        WordListView.Items.Add(CiYu)
                    End If
                End While
            End Using
        Next

        WordListView.EndUpdate()
    End Sub
    ''' <summary>
    ''' 保存词语文件
    ''' </summary>
    ''' <param name="FileName">文件路径</param>
    ''' <remarks></remarks>
    Public Sub SaveFile(FileName As String)
        Using WriterText As New StreamWriter(FileName, False, System.Text.Encoding.Default)
            For Each ListItem As ListViewItem In WordListView.Items
                WriterText.WriteLine(ListItem.Text)
            Next
        End Using
    End Sub
    Private Sub OpenButton_Click(sender As Object, e As EventArgs) Handles OpenButton.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            OpenFiles(OpenFileDialog1.FileNames)
        End If
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        WordListView.Items.Clear()
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        WordListView.BeginUpdate()
        For Each SelectedItem As ListViewItem In WordListView.SelectedItems
            SelectedItem.Remove()
        Next
        WordListView.EndUpdate()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Dim AddString As String = ""
        AddString = InputBox("要添加的词语：", "添加词语")?.Trim()
        While Not String.IsNullOrEmpty(AddString)
            WordListView.Items.Add(AddString)
            AddString = InputBox("要添加的词语：", "添加词语")?.Trim()
        End While
    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click
        If WordListView.SelectedItems.Count > 0 Then
            WordListView.SelectedItems.Item(0).BeginEdit()
        End If
    End Sub
    Private Sub UpButton_Click(sender As Object, e As EventArgs) Handles UpButton.Click
        Dim SelectedItems As ListView.SelectedListViewItemCollection = WordListView.SelectedItems
        If SelectedItems.Count > 0 Then
            If SelectedItems.Item(0).Index > 0 Then
                WordListView.BeginUpdate()

                For Each SelectedItem As ListViewItem In SelectedItems
                    Dim SelectedItemIndex As Integer = SelectedItem.Index
                    SelectedItem.Remove()
                    WordListView.Items.Insert(SelectedItemIndex - 1, SelectedItem).EnsureVisible()
                Next

                WordListView.EndUpdate()
            End If
        End If
    End Sub
    Private Sub DownButton_Click(sender As Object, e As EventArgs) Handles DownButton.Click
        Dim SelectedItems As ListView.SelectedListViewItemCollection = WordListView.SelectedItems
        If SelectedItems.Count > 0 Then
            If SelectedItems.Item(SelectedItems.Count - 1).Index < WordListView.Items.Count - 1 Then
                WordListView.BeginUpdate()

                For i = SelectedItems.Count - 1 To 0 Step -1
                    Dim SelectedItem As ListViewItem = SelectedItems.Item(i)
                    Dim SelectedItemIndex As Integer = SelectedItem.Index
                    SelectedItem.Remove()
                    WordListView.Items.Insert(SelectedItemIndex + 1, SelectedItem).EnsureVisible()
                Next

                WordListView.EndUpdate()
            End If
        End If
    End Sub
    Private Sub CountButton_Click(sender As Object, e As EventArgs) Handles CountButton.Click
        MessageBox.Show("词语数量：" & WordListView.Items.Count.ToString())
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Call OpenButton_Click(OpenButton, EventArgs.Empty)
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SaveFile(SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If WordListView.Items.Count = 0 Then
            MessageBox.Show("请先添加词语！")
            Exit Sub
        End If
        If BoBao.GetNextWordIndex() >= WordListView.Items.Count Then
            MessageBox.Show("已经播完了。")
            Exit Sub
        End If
        Button3.Text = "暂停自动播报(&P)"
        BoBao.SpeakNext()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        BoBao.StopThis()
        BASS.FreeAllPlugin()
        BASS.Free()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If BASS.Init(-1, 44100, 0, Handle) Then
            Try
                Dim pluginDir As New DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase & "bass_plugin")
                For Each File In pluginDir.GetFiles()
                    BASS.LoadPlugin(File.FullName)
                Next
            Catch ex As Exception

            End Try
            CiZuZengQiangMuLu = GetSetting("自动默写", "选项", "词组增强", "")
        Else
            设置词组增强ToolStripMenuItem.Enabled = False
            设置词组增强ToolStripMenuItem.Text = "加载词组增强功能失败"
        End If
        BoBao.Speaker = New Speaker()
        BoBao.WordListSource = New WordListAdapterForListView(WordListView)
        InitVoiceList()
        Try
            ComboBox1.SelectedIndex = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "引擎", "0"))
        Catch ex As Exception
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End Try

        ReadFontSettings()
        自动翻页ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "自动翻页", Boolean.TrueString))
        字词跟随ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "字词跟随", Boolean.TrueString))

        TrackBar1.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "音量", "100"))
        TrackBar1_Scroll(TrackBar1, EventArgs.Empty)

        TrackBar2.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "语速", "0"))
        TrackBar2_Scroll(TrackBar2, EventArgs.Empty)

        ComboBox1_SelectedIndexChanged(ComboBox1, EventArgs.Empty)

        For Each CommandLine As String In My.Application.CommandLineArgs
            If File.Exists(CommandLine) Then
                OpenFiles({CommandLine})
            End If
        Next

    End Sub
    Private Sub InitVoiceList()
        For Each NativeEngine As Object In MicrosoftSpeechEngine.GetAllNativeEngines()
            SpeakEngines.Add(New MicrosoftSpeechEngine(NativeEngine))
        Next

        Dim EnglishEngineLevel As Integer '用于记录英文语音引擎等级，越高表示引擎越好
        Dim ChineseEngineLevel As Integer '用于记录中文语音引擎等级，越高表示引擎越好
        For Each SpeakEngine As ISpeakEngine In SpeakEngines
            Dim EngineName As String = SpeakEngine.Name
            If SpeakEngine.Culture IsNot Nothing Then
                If SpeakEngine.Culture.LCID = 1033 Or SpeakEngine.Culture.LCID = 2057 Then
                    If Button11.Tag Is Nothing OrElse EnglishEngineLevel < GetEnglishEngineLevel(EngineName) Then
                        Button11.Tag = (ComboBox1.Items.Count)
                        EnglishEngineLevel = GetEnglishEngineLevel(EngineName)
                    End If
                    If SpeakEngine.Culture.LCID = 1033 Then
                        EngineName = "（US）" & EngineName
                    ElseIf SpeakEngine.Culture.LCID = 2057 Then
                        EngineName = "（UK）" & EngineName
                    End If
                ElseIf SpeakEngine.Culture.LCID = 2052 Then
                    If Button10.Tag Is Nothing OrElse ChineseEngineLevel < GetChineseEngineLevel(EngineName) Then
                        Button10.Tag = (ComboBox1.Items.Count)
                        ChineseEngineLevel = GetChineseEngineLevel(EngineName)
                    End If
                    EngineName = "（CN）" & EngineName
                End If
            End If
            ComboBox1.Items.Add(EngineName)
        Next
    End Sub
    Private Sub ReadFontSettings()
        Dim ListViewFontName As String = GetSetting("自动默写", "字体", "名称", WordListView.Font.Name)
        Dim ListViewFontSize As Single = Convert.ToSingle(GetSetting("自动默写", "字体", "大小", WordListView.Font.Size.ToString))
        Dim ListViewFontGdiVertical As Boolean = Convert.ToBoolean(GetSetting("自动默写", "字体", "垂直", WordListView.Font.GdiVerticalFont.ToString))
        Dim ListViewFontFontGdiCharSet As Byte = Convert.ToByte(GetSetting("自动默写", "字体", "字符集", WordListView.Font.GdiCharSet.ToString))
        Dim ListViewFontStyle As FontStyle = CType(GetSetting("自动默写", "字体", "样式", Convert.ToInt32(WordListView.Font.Style).ToString), FontStyle)
        WordListView.Font = New Font(ListViewFontName, ListViewFontSize, ListViewFontStyle, WordListView.Font.Unit, ListViewFontFontGdiCharSet, ListViewFontGdiVertical)
        WordListView.ForeColor = Color.FromArgb(Convert.ToInt32(GetSetting("自动默写", "字体", "颜色", WordListView.ForeColor.ToArgb.ToString)))
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If (BoBao.GetNextWordIndex() <= WordListView.Items.Count) And (BoBao.GetNextWordIndex() > 0) Then
            Button3.Text = "暂停自动播报(&P)"
            BoBao.SpeakAgain()
        Else
            MessageBox.Show(Me, "还没报过或已移除报过的词语！")
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If BoBao.GetNextWordIndex() <= WordListView.Items.Count And BoBao.GetNextWordIndex() > 1 Then
            Button3.Text = "暂停自动播报(&P)"
            BoBao.SpeakLast()
        Else
            MessageBox.Show(Me, "已经是第1个了！")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not BoBao.IsAutoMode Then
            BoBao.ResetProgress()
            WordListView.BeginUpdate()
            For Each ListItem As ListViewItem In WordListView.Items
                ListItem.BackColor = Color.Empty
            Next
            WordListView.EndUpdate()
        Else
            MessageBox.Show(Me, "请先停止自动播报！")
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If WordListView.Items.Count = 0 Then
            Exit Sub
        End If
        If BoBao.IsAutoMode Then
            BoBao.StopAuto()
        End If
        Button5_Click(Button1, EventArgs.Empty) '记录归零
        BoBao.StartAuto()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If BoBao.IsAutoMode Then
            BoBao.StopAuto()
        ElseIf BoBao.IsSpeaking Then
            BoBao.StopThis()
        Else
            Exit Sub
        End If
        BoBao_StoppedThis(BoBao.GetNextWordIndex() - 1, False)
    End Sub

    Private Sub 保存SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存SToolStripMenuItem.Click
        Call SaveButton_Click(保存SToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 退出EToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出EToolStripMenuItem.Click
        End
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Button9.Text = "隐藏词语列表" Then
            WordListView.Visible = False
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
            WordListView.Visible = True
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

    Private Sub 上移ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 上移ToolStripMenuItem.Click
        UpButton_Click(上移ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 下移ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 下移ToolStripMenuItem.Click
        DownButton_Click(下移ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 修改ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 修改ToolStripMenuItem.Click
        EditButton_Click(修改ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 移除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 移除ToolStripMenuItem.Click
        DeleteButton_Click(移除ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 反向选择ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 反向选择ToolStripMenuItem.Click
        反向选择ToolStripMenuItem1_Click(反向选择ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 显示隐藏词语列表ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 显示隐藏词语列表ToolStripMenuItem.Click
        Call Button9_Click(显示隐藏词语列表ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 添加词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 添加词语ToolStripMenuItem.Click
        Call AddButton_Click(添加词语ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 移除选中词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 移除选中词语ToolStripMenuItem.Click
        Call DeleteButton_Click(移除选中词语ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 清空词语列表ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清空词语列表ToolStripMenuItem.Click
        Call ClearButton_Click(清空词语列表ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 修改选中词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 修改选中词语ToolStripMenuItem.Click
        Call EditButton_Click(修改选中词语ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 向上移动ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 向上移动ToolStripMenuItem.Click
        Call UpButton_Click(向上移动ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 向下移动ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 向下移动ToolStripMenuItem.Click
        Call DownButton_Click(向下移动ToolStripMenuItem, EventArgs.Empty)
    End Sub

    Private Sub 反向选择ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 反向选择ToolStripMenuItem1.Click
        For Each ListItem As ListViewItem In WordListView.Items
            ListItem.Selected = Not ListItem.Selected
        Next
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button10.Tag Is Nothing Then
            MessageBox.Show("未发现中文语音引擎！")
            Exit Sub
        End If
        ComboBox1.SelectedIndex = CType(Button10.Tag, Int32)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Button11.Tag Is Nothing Then
            MessageBox.Show("未发现英文语音引擎！")
            Exit Sub
        End If
        ComboBox1.SelectedIndex = CType(Button11.Tag, Int32)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        Dim Engine As ISpeakEngine = Nothing
        If ComboBox1.SelectedIndex <> -1 Then
            Engine = SpeakEngines(ComboBox1.SelectedIndex)
        End If
        If CiZuZengQiangMuLu IsNot Nothing AndAlso CiZuZengQiangMuLu.Trim() <> "" Then
            Engine = New ImprovedSpeakEngine(Engine, CiZuZengQiangMuLu)
        End If
        CType(BoBao.Speaker, Speaker).Engine = Engine
        SaveSetting("自动默写", "TTS引擎", "引擎", ComboBox1.SelectedIndex.ToString)
    End Sub

    Private Sub 设置词组增强ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 设置词组增强ToolStripMenuItem.Click
        If SetImprovedSpeakEngineDialog.ShowDialog() = DialogResult.OK Then
            ComboBox1_SelectedIndexChanged(ComboBox1, EventArgs.Empty)
        End If
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
        AboutBox.ShowDialog()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        For Each SpeakEngineObject In SpeakEngines
            CType(BoBao.Speaker, Speaker).Volume = CByte(TrackBar1.Value)
        Next
        SaveSetting("自动默写", "TTS引擎", "音量", CByte(TrackBar1.Value).ToString)
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        For Each SpeakEngineObject In SpeakEngines
            CType(BoBao.Speaker, Speaker).Rate = CSByte(TrackBar2.Value)
        Next
        SaveSetting("自动默写", "TTS引擎", "语速", CSByte(TrackBar2.Value).ToString)
    End Sub


    Private Sub 全选ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem1.Click
        全选ToolStripMenuItem_Click(全选ToolStripMenuItem1, EventArgs.Empty)
    End Sub

    Private Sub 全选ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem.Click
        For Each ListItem As ListViewItem In WordListView.Items
            ListItem.Selected = True
        Next
    End Sub

    Private Sub ZiDongBoBaoJianGeTextBox_TextChanged(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox.TextChanged
        If ZiDongBoBaoJianGeTextBox.Text = "" Then
            ZiDongBoBaoJianGeTextBox.Text = "0"
            ZiDongBoBaoJianGeTextBox.Select(0, 1)
        End If

        Dim WaitingTimeCalculator As IWaitingTimeCalculator
        Try
            WaitingTimeCalculator = New WaitingTimeAccordingToExpression(ZiDongBoBaoJianGeTextBox.Text)
            WaitingTimeCalculator.CalculateWaitingTime("测试 Test")
            ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "")
        Catch ex As Exception
            WaitingTimeCalculator = New FixedWaitingTime(0)
            ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "表达式有误")
        End Try
        BoBao.CurrentWaitingTimeCalculator = WaitingTimeCalculator
    End Sub

    Private Sub ZiDongBoBaoCiShuTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ZiDongBoBaoCiShuTextBox.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
    Private Sub ZiDongBoBaoCiShuTextBox_TextChanged(sender As Object, e As EventArgs) Handles ZiDongBoBaoCiShuTextBox.TextChanged
        If ZiDongBoBaoCiShuTextBox.Text = "" OrElse CInt(ZiDongBoBaoCiShuTextBox.Text) < 1 Then
            ZiDongBoBaoCiShuTextBox.Text = "1"
            ZiDongBoBaoCiShuTextBox.Select(0, 1)
        End If
        BoBao.AutoMode_TimesPerWord = Convert.ToInt32(ZiDongBoBaoCiShuTextBox.Text)
    End Sub

    Private Sub 随机排序ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机排序ToolStripMenuItem.Click
        WordListView.BeginUpdate()

        Dim rand = New Random()
        For EndIndex As Integer = WordListView.Items.Count To 2 Step -1
            Dim ListItem As ListViewItem
            Dim xxczdsyh As Integer
            xxczdsyh = rand.Next(0, EndIndex)
            ListItem = WordListView.Items.Item(xxczdsyh)
            ListItem.Remove()
            WordListView.Items.Add(ListItem)
        Next

        WordListView.EndUpdate()
    End Sub

    Private Sub 随机选词ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机选词ToolStripMenuItem.Click
        RandomExtractionForm.Show(Me)
    End Sub


    Private Sub 字体FToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 字体FToolStripMenuItem.Click
        Dim FontReturnValue As DialogResult

        FontDialog1.Font = WordListView.Font
        FontDialog1.Color = WordListView.ForeColor

        FontReturnValue = FontDialog1.ShowDialog
        If FontReturnValue = DialogResult.OK Then
            WordListView.Font = FontDialog1.Font
            WordListView.ForeColor = FontDialog1.Color
            SaveSetting("自动默写", "字体", "名称", WordListView.Font.Name)
            SaveSetting("自动默写", "字体", "大小", WordListView.Font.Size.ToString)
            SaveSetting("自动默写", "字体", "样式", Convert.ToInt32(WordListView.Font.Style).ToString)
            SaveSetting("自动默写", "字体", "垂直", WordListView.Font.GdiVerticalFont.ToString)
            SaveSetting("自动默写", "字体", "字符集", WordListView.Font.GdiCharSet.ToString)
            SaveSetting("自动默写", "字体", "颜色", WordListView.ForeColor.ToArgb.ToString)
        End If
    End Sub

    Private Sub 读选定词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 读选定词语ToolStripMenuItem.Click
        Dim SelectedIndex As Integer
        If WordListView.SelectedIndices.Count > 0 Then
            SelectedIndex = WordListView.SelectedIndices.Item(0)
            Button3.Text = "暂停自动播报(&P)"
            BoBao.Speak(SelectedIndex)
        End If
    End Sub
    Private Sub WordListView_BeforeLabelEdit(sender As Object, e As LabelEditEventArgs) Handles WordListView.BeforeLabelEdit
        移除ToolStripMenuItem.Enabled = False
    End Sub
    Private Sub WordListView_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles WordListView.AfterLabelEdit
        If e.Label IsNot Nothing Then
            If e.Label.Trim() = "" Then
                e.CancelEdit = True
            End If
        End If
        移除ToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If BoBao.IsAutoMode = False Then
            MessageBox.Show("当前不在自动播报！")
            Exit Sub
        End If
        If Button3.Text = "暂停自动播报(&P)" Then
            BoBao.PauseAuto()
            Label4.Text = "自动播报已暂停"
            Button3.Text = "继续自动播报(&R)"
        Else
            BoBao.ResumeAuto()
            Button3.Text = "暂停自动播报(&P)"
        End If
    End Sub

    Private Sub BoBao_StoppedThis(i As Integer, Completed As Boolean) Handles BoBao.StoppedThis
        WordListView.Invoke(Sub()
                                读选定词语ToolStripMenuItem.Enabled = True
                                If Not BoBao.IsAutoMode Then
                                    Label4.Text = "等待播报"
                                End If
                                Try
                                    Label1.Text = "本词语播报次数：" & BoBao.ElapsedTimes
                                Catch ex As Exception
                                    Label1.Text = "本词语播报次数：0"
                                End Try
                            End Sub)
    End Sub

    Private Sub BoBao_Speaking(i As Integer) Handles BoBao.Speaking
        WordListView.Invoke(Sub()
                                Label4.Text = "正在播报第" & (i + 1).ToString() & "个"
                                Label1.Text = "本词语播报次数：" & BoBao.ElapsedTimes
                                If 字词跟随ToolStripMenuItem.Checked = True Then
                                    Dim BackColor = Color.FromArgb(&H33, &H66, &HFF)
                                    If WordListView.Items(i).BackColor <> BackColor Then
                                        For Each ListItem As ListViewItem In WordListView.Items
                                            If ListItem.BackColor = BackColor Then
                                                ListItem.BackColor = Color.Empty
                                            End If
                                        Next
                                        WordListView.Items(i).BackColor = BackColor
                                    End If
                                End If
                                If 自动翻页ToolStripMenuItem.Checked = True Then
                                    WordListView.Items(i).EnsureVisible()
                                End If
                            End Sub)
    End Sub

    Private Sub BoBao_UpdateWaitingTimeInfo(i As Integer, jiangge As Integer) Handles BoBao.UpdateWaitingTimeInfo
        WordListView.Invoke(Sub() Label4.Text = jiangge.ToString() & "秒后自动播报第" & (i + 1).ToString() & "个")
    End Sub

    Private Sub BoBao_AutoSpeakingCompleted(Reason As Integer) Handles BoBao.AutoSpeakingCompleted
        WordListView.Invoke(Sub()
                                Button4.Enabled = True
                                Button8.Enabled = True
                                Button7.Enabled = True
                                读选定词语ToolStripMenuItem.Enabled = True
                                Label4.Text = "等待播报"
                                If Reason = 0 Then
                                    MessageBox.Show(Me, "自动播报已完成！")
                                End If
                                Button3.Text = "暂停自动播报(&P)"
                            End Sub)
    End Sub

    Private Sub 在Bing词典中查看ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 在Bing词典中查看ToolStripMenuItem.Click
        Dim SelectedItemIndex As Integer
        Dim SelectedItemText As String
        If WordListView.SelectedIndices.Count > 0 Then
            SelectedItemIndex = WordListView.SelectedIndices.Item(0)
            SelectedItemText = WordListView.Items.Item(SelectedItemIndex).Text
            Process.Start("https://cn.bing.com/dict/search?q=" & UrlEncode_UTF8(SelectedItemText) & "&qs=n")
        End If
    End Sub
    Public Shared Function UrlEncode_UTF8(s As String) As String
        Dim sb As Text.StringBuilder = New Text.StringBuilder()
        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(s)
        For Each b As Byte In bytes
            sb.AppendFormat("%{0:X2}", b)
        Next
        Return sb.ToString()
    End Function

    Private Sub WordListView_DragDrop(sender As Object, e As DragEventArgs) Handles WordListView.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            OpenFiles(files)
        End If
    End Sub

    Private Sub WordListView_DragOver(sender As Object, e As DragEventArgs) Handles WordListView.DragOver
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.Undo()
    End Sub
    Private Sub ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.Cut()
    End Sub
    Private Sub ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.Copy()
    End Sub
    Private Sub ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.Paste()
    End Sub
    Private Sub ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.SelectedText = ""
    End Sub
    Private Sub ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.SelectAll()
    End Sub
    Private Sub 插入LengthToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 插入LengthToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.SelectedText = "[Length]"
    End Sub
    Private Sub 插入WordCountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 插入WordCountToolStripMenuItem.Click
        ZiDongBoBaoJianGeTextBox.SelectedText = "[WordCount]"
    End Sub

    Private Sub 保存音频ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存音频ToolStripMenuItem.Click
        SaveAudioDialog.ShowDialog(Me)
    End Sub

    Private Sub 插入ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 插入ToolStripMenuItem.Click
        If WordListView.SelectedItems.Count > 0 Then
            Dim item = WordListView.Items.Insert(WordListView.SelectedItems.Item(0).Index, "Please edit")
            WordListView.SelectedItems.Item(0).Selected = False
            item.BeginEdit()
        End If
    End Sub

    Private Sub 从此处开始自动播报ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 从此处开始自动播报ToolStripMenuItem.Click
        If WordListView.SelectedItems.Count > 0 Then
            If BoBao.IsAutoMode Then
                BoBao.StopAuto()
            End If
            Button5_Click(Button1, EventArgs.Empty) '记录归零
            BoBao.StartAuto(WordListView.SelectedItems.Item(0).Index)
        End If
    End Sub
End Class
