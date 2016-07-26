Imports System.ComponentModel
Imports System.IO

Public Class Form1
    Dim WithEvents SpVoice1 As New SpeechLib.SpVoice
    Dim WithEvents BoBao As New BoBaoLuoJi
    Dim WithEvents AudioFileStream As BASS_Stream

    Dim ChuFaSpVoice1EndStream As Boolean = True '触发SpVoice1EndStream


    ''' <summary>
    ''' 词组增强支持的音频文件扩展名
    ''' </summary>
    ''' <remarks></remarks>
    Dim AudioFileExtension As String() = Split("wav;ape;flac;m4a;mp3;mp2;mp1;ogg;aif", ";")

    Dim EnglishVoiceGood As Boolean = False '是否安装了语音质量良好的英文语音引擎
    Dim ChineseVoiceGood As Boolean = False '是否安装了语音质量良好的中文语音引擎

    ''' <summary>
    ''' 打开多个词语文件
    ''' </summary>
    ''' <param name="Files">词语文件列表（Ansi编码）</param>
    ''' <remarks>不会在列表中移除原词语</remarks>
    Public Sub OpenFiles(Files As String())
        CiYuListView.BeginUpdate()

        For Each FileName As String In Files
            Dim ReadText As New StreamReader(FileName, System.Text.Encoding.Default)
            While Not ReadText.EndOfStream
                Dim CiYu As String = ReadText.ReadLine()
                If CiYu.Trim() <> "" Then
                    CiYuListView.Items.Add(CiYu).SubItems.Add("0")
                End If
            End While
            ReadText.Dispose()
        Next

        CiYuListView.EndUpdate()
    End Sub
    ''' <summary>
    ''' 保存词语文件
    ''' </summary>
    ''' <param name="FileName">文件路径</param>
    ''' <remarks></remarks>
    Public Sub SaveFile(FileName As String)
        Dim WriterText As New StreamWriter(FileName, False, System.Text.Encoding.Default)
        For Each ListItem As ListViewItem In CiYuListView.Items
            WriterText.WriteLine(ListItem.Text)
        Next
        WriterText.Dispose()
    End Sub
    Private Sub OpenButton_Click(sender As Object, e As EventArgs) Handles OpenButton.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            OpenFiles(OpenFileDialog1.FileNames)
        End If
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        CiYuListView.Items.Clear()
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        For Each SelectedItem As System.Windows.Forms.ListViewItem In CiYuListView.SelectedItems
            SelectedItem.Remove()
        Next
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Dim AddString As String
        AddString = InputBox("要添加的词语：", "添加词语")
        If AddString.Trim() <> "" Then
            CiYuListView.Items.Add(AddString).SubItems.Add("0")
        End If
    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click
        If CiYuListView.SelectedItems.Count > 0 Then
            CiYuListView.SelectedItems.Item(0).BeginEdit()
        End If
    End Sub
    Private Sub UpButton_Click(sender As Object, e As EventArgs) Handles UpButton.Click
        Dim SelectedItems As ListView.SelectedListViewItemCollection = CiYuListView.SelectedItems
        If SelectedItems.Count > 0 Then
            If SelectedItems.Item(0).Index > 0 Then
                CiYuListView.BeginUpdate()

                For Each SelectedItem As ListViewItem In SelectedItems
                    Dim SelectedItemIndex As Integer = SelectedItem.Index
                    SelectedItem.Remove()
                    CiYuListView.Items.Insert(SelectedItemIndex - 1, SelectedItem).EnsureVisible()
                Next

                CiYuListView.EndUpdate()
            End If
        End If
    End Sub
    Private Sub DownButton_Click(sender As Object, e As EventArgs) Handles DownButton.Click
        Dim SelectedItems As ListView.SelectedListViewItemCollection = CiYuListView.SelectedItems
        If SelectedItems.Count > 0 Then
            If SelectedItems.Item(SelectedItems.Count - 1).Index < CiYuListView.Items.Count - 1 Then
                CiYuListView.BeginUpdate()

                For i = SelectedItems.Count - 1 To 0 Step -1
                    Dim SelectedItem As ListViewItem = SelectedItems.Item(i)
                    Dim SelectedItemIndex As Integer = SelectedItem.Index
                    SelectedItem.Remove()
                    CiYuListView.Items.Insert(SelectedItemIndex + 1, SelectedItem).EnsureVisible()
                Next

                CiYuListView.EndUpdate()
            End If
        End If
    End Sub
    Private Sub CountButton_Click(sender As Object, e As EventArgs) Handles CountButton.Click
        MessageBox.Show("词语数量：" & CiYuListView.Items.Count.ToString())
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Call OpenButton_Click(OpenButton, System.EventArgs.Empty)
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SaveFile(SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CiYuListView.Items.Count = 0 Then
            MessageBox.Show("请先添加词语！")
            Exit Sub
        End If
        If BoBao.GetXiaYiGeWeiZhi() >= CiYuListView.Items.Count Then
            MessageBox.Show("已经播完了。")
            Exit Sub
        End If
        Button3.Text = "暂停自动播报(&P)"
        BoBao.BoBaoXiaYiGe()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If AudioFileStream IsNot Nothing Then
            AudioFileStream.Dispose()
            AudioFileStream = Nothing
        End If
        BASS.FreeAllPlugin()
        BASS.Free()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadFontSettings()
        自动翻页ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "自动翻页", System.Boolean.TrueString))
        字词跟随ToolStripMenuItem.Checked = Convert.ToBoolean(GetSetting("自动默写", "选项", "字词跟随", System.Boolean.TrueString))
        CiZuZengQiangMuLu = GetSetting("自动默写", "选项", "词组增强", "")
        InitVoiceList()

        TrackBar1.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "音量", "100"))
        TrackBar1_Scroll(TrackBar1, Nothing)

        TrackBar2.Value = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "语速", "0"))
        TrackBar2_Scroll(TrackBar2, Nothing)

        Try
            ComboBox1.SelectedIndex = Convert.ToInt32(GetSetting("自动默写", "TTS引擎", "引擎", "0"))
        Catch ex As Exception
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End Try

        For Each CommandLine As String In My.Application.CommandLineArgs
            If System.IO.File.Exists(CommandLine) = True Then
                OpenFiles({CommandLine})
            End If
        Next

        BASS.Init(-1, 44100, 0, Me.Handle)
        BASS.LoadPlugin("bassflac.dll")
        BASS.LoadPlugin("bass_ape.dll")
        BASS.LoadPlugin("bass_alac.dll")
    End Sub
    Private Sub InitVoiceList()
        Dim EnglishGrade As Integer '声明用于记录英文语音引擎质量等级的变量
        Dim ChineseGrade As Integer '声明用于记录中文语音引擎质量等级的变量
        For Each Voice As SpeechLib.SpObjectToken In SpVoice1.GetVoices
            Dim VoiceName As String
            VoiceName = Voice.GetAttribute("name") '用GetDescription在部分罕见的引擎会出错
            If IsVoiceLanguageIDEquals(Voice, 1033) Then
                If (EnglishGrade = 0) Or (EnglishGrade > GetEnglishGrade(VoiceName) And GetEnglishGrade(VoiceName) <> 0) Then
                    Button11.Tag = (ComboBox1.Items.Count)
                    EnglishGrade = GetEnglishGrade(VoiceName)
                End If
                VoiceName = "（英）" & VoiceName
            ElseIf IsVoiceLanguageIDEquals(Voice, 2052) Then
                If (ChineseGrade = 0) Or (ChineseGrade > GetChineseGrade(VoiceName) And GetChineseGrade(VoiceName) <> 0) Then
                    Button10.Tag = (ComboBox1.Items.Count)
                    ChineseGrade = GetChineseGrade(VoiceName)
                End If
                VoiceName = "（中）" & VoiceName
            End If
            ComboBox1.Items.Add(VoiceName)
        Next

        EnglishVoiceGood = EnglishGrade > 0
        ChineseVoiceGood = ChineseGrade > 0 And EnglishGrade < 5
    End Sub
    Private Sub ReadFontSettings()
        Dim ListViewFontName As String = GetSetting("自动默写", "字体", "名称", CiYuListView.Font.Name)
        Dim ListViewFontSize As Single = Convert.ToSingle(GetSetting("自动默写", "字体", "大小", CiYuListView.Font.Size.ToString))
        Dim ListViewFontGdiVertical As Boolean = Convert.ToBoolean(GetSetting("自动默写", "字体", "垂直", CiYuListView.Font.GdiVerticalFont.ToString))
        Dim ListViewFontFontGdiCharSet As Byte = Convert.ToByte(GetSetting("自动默写", "字体", "字符集", CiYuListView.Font.GdiCharSet.ToString))
        Dim ListViewFontStyle As FontStyle = CType(GetSetting("自动默写", "字体", "样式", Convert.ToInt32(CiYuListView.Font.Style).ToString), FontStyle)
        CiYuListView.Font = New Font(ListViewFontName, ListViewFontSize, ListViewFontStyle, CiYuListView.Font.Unit, ListViewFontFontGdiCharSet, ListViewFontGdiVertical)
        CiYuListView.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(GetSetting("自动默写", "字体", "颜色", CiYuListView.ForeColor.ToArgb.ToString)))
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If (BoBao.GetXiaYiGeWeiZhi() <= CiYuListView.Items.Count) And (BoBao.GetXiaYiGeWeiZhi() > 0) Then
            Button3.Text = "暂停自动播报(&P)"
            BoBao.BoBaoDangQian()
        Else
            MessageBox.Show("还没报过或已移除报过的词语！")
        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If BoBao.GetXiaYiGeWeiZhi() <= CiYuListView.Items.Count And BoBao.GetXiaYiGeWeiZhi() > 1 Then
            Button3.Text = "暂停自动播报(&P)"
            BoBao.BoBaoShangYiGe()
        Else
            MessageBox.Show("已经是第1个了！")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not BoBao.IsZddb() Then
            BoBao.CongTouKaiShi()
            For Each ListItem As System.Windows.Forms.ListViewItem In CiYuListView.Items
                ListItem.SubItems.Item(1).Text = "0"
            Next
        Else
            MessageBox.Show("请先停止自动播报！")
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CiYuListView.Items.Count = 0 Then
            MessageBox.Show("请先添加词语！")
            Exit Sub
        End If
        Button1.Enabled = False
        Button5_Click(Button1, System.EventArgs.Empty) '记录归零
        BoBao.KaiShiZiDongBoBao()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If BoBao.IsZddb() Then
            BoBao.JieShuZiDongBoBao()
            BoBaoWanBi()
            MessageBox.Show("自动播报已结束！")
        Else
            BoBao_TingZhiDangQianBoBao()
            BoBaoWanBi()
            MessageBox.Show("播报已结束！")
        End If
    End Sub

    Private Sub 保存SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存SToolStripMenuItem.Click
        Call SaveButton_Click(保存SToolStripMenuItem, System.EventArgs.Empty)
    End Sub

    Private Sub 退出EToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出EToolStripMenuItem.Click
        End
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Button9.Text = "隐藏词语列表" Then
            CiYuListView.Visible = False
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
            CiYuListView.Visible = True
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

        If GroupBox3.Height < 172 Then
            Temp = Convert.ToInt32((GroupBox3.Height - 47) / 4)
            Button3.Font = New System.Drawing.Font("微软雅黑", 9.0!)
            Button3.Height = Temp
        Else
            Temp = Convert.ToInt32((GroupBox3.Height - 47) / 5)
            Button3.Font = New System.Drawing.Font("微软雅黑", 18.0!)
            Button3.Height = Temp * 2
        End If

        Button1.Height = Temp
        Button2.Height = Temp

        Button3.Top = Button1.Top + Button1.Height + 6

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

    Private Sub 移除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 移除ToolStripMenuItem.Click
        DeleteButton_Click(移除ToolStripMenuItem, System.EventArgs.Empty)
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

    Private Sub 移除选中词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 移除选中词语ToolStripMenuItem.Click
        Call DeleteButton_Click(移除选中词语ToolStripMenuItem, System.EventArgs.Empty)
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
        For Each ListItem As System.Windows.Forms.ListViewItem In CiYuListView.Items
            ListItem.Selected = Not ListItem.Selected
        Next
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button10.Tag Is Nothing Then
            If MessageBox.Show("未发现中文语音引擎！请下载中文引擎！", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                System.Diagnostics.Process.Start("http://pan.baidu.com/share/link?shareid=454161&uk=553949470")
            End If
            Exit Sub
        End If
        If ChineseVoiceGood = False Then
            If MessageBox.Show("建议安装更好的中文语音引擎！是否安装？", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                System.Diagnostics.Process.Start("http://pan.baidu.com/share/link?shareid=454161&uk=553949470")
            End If
        End If
        ComboBox1.SelectedIndex = CType(Button10.Tag, Int32)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Button11.Tag Is Nothing Then
            If MessageBox.Show("未发现英文语音引擎！请下载英文引擎！", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                System.Diagnostics.Process.Start("http://pan.baidu.com/share/link?shareid=454161&uk=553949470")
            End If
            Exit Sub
        End If
        If EnglishVoiceGood = False Then
            If MessageBox.Show("建议安装更好的英文语音引擎！是否安装？", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                System.Diagnostics.Process.Start("http://pan.baidu.com/share/link?shareid=454161&uk=553949470")
            End If
        End If
        ComboBox1.SelectedIndex = CType(Button11.Tag, Int32)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SpVoice1.Voice = SpVoice1.GetVoices.Item(ComboBox1.SelectedIndex)
        SaveSetting("自动默写", "TTS引擎", "引擎", ComboBox1.SelectedIndex.ToString)
    End Sub

    Private Sub 设置词组增强ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 设置词组增强ToolStripMenuItem.Click
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
        BASS.SetVolume(TrackBar1.Value / 100.0F)
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
        For Each ListItem As System.Windows.Forms.ListViewItem In CiYuListView.Items
            ListItem.Selected = True
        Next
    End Sub

    Private Sub ZiDongBoBaoJianGeTextBox_TextChanged(sender As Object, e As EventArgs) Handles ZiDongBoBaoJianGeTextBox.TextChanged
        If ZiDongBoBaoJianGeTextBox.Text = "" Then
            ZiDongBoBaoJianGeTextBox.Text = "0"
            ZiDongBoBaoJianGeTextBox.Select(0, 1)
        End If

        Dim BoBaoJianGeObject As BoBaoJianGe
        Try
            BoBaoJianGeObject = New BoBaoJianGe_BiaoDaShi(ZiDongBoBaoJianGeTextBox.Text)
            BoBaoJianGeObject.GetBoBoJianGe(New CiYuXinXi("测试 Test"))
            ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "")
        Catch ex As Exception
            BoBaoJianGeObject = New BoBaoJianGe_GuDing(0)
            ErrorProvider1.SetError(ZiDongBoBaoJianGeTextBox, "表达式有误")
        End Try
        BoBao.SetJianGe(BoBaoJianGeObject)
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
        BoBao.ZiDongBoBao_MeiCiBianShu = Convert.ToInt32(ZiDongBoBaoCiShuTextBox.Text)
    End Sub

    Private Sub 随机排序ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机排序ToolStripMenuItem.Click
        CiYuListView.BeginUpdate()

        Dim rand = New Random()
        For EndIndex As Integer = CiYuListView.Items.Count To 2 Step -1
            Dim ListItem As ListViewItem
            Dim xxczdsyh As Integer
            xxczdsyh = rand.Next(0, EndIndex)
            ListItem = CiYuListView.Items.Item(xxczdsyh)
            ListItem.Remove()
            CiYuListView.Items.Add(ListItem)
        Next

        CiYuListView.EndUpdate()
    End Sub

    Private Sub 随机选词ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 随机选词ToolStripMenuItem.Click
        Form3.Show(Me)
    End Sub


    Private Sub 字体FToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 字体FToolStripMenuItem.Click
        Dim FontReturnValue As DialogResult

        FontDialog1.Font = CiYuListView.Font
        FontDialog1.Color = CiYuListView.ForeColor

        FontReturnValue = FontDialog1.ShowDialog
        If FontReturnValue = DialogResult.OK Then
            CiYuListView.Font = FontDialog1.Font
            CiYuListView.ForeColor = FontDialog1.Color
            SaveSetting("自动默写", "字体", "名称", CiYuListView.Font.Name)
            SaveSetting("自动默写", "字体", "大小", CiYuListView.Font.Size.ToString)
            SaveSetting("自动默写", "字体", "样式", Convert.ToInt32(CiYuListView.Font.Style).ToString)
            SaveSetting("自动默写", "字体", "垂直", CiYuListView.Font.GdiVerticalFont.ToString)
            SaveSetting("自动默写", "字体", "字符集", CiYuListView.Font.GdiCharSet.ToString)
            SaveSetting("自动默写", "字体", "颜色", CiYuListView.ForeColor.ToArgb.ToString)
        End If
    End Sub

    Private Sub 读选定词语ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 读选定词语ToolStripMenuItem.Click
        Dim SelectedIndex As Integer
        If CiYuListView.SelectedIndices.Count > 0 Then
            SelectedIndex = CiYuListView.SelectedIndices.Item(0)
            Button3.Text = "暂停自动播报(&P)"
            BoBao.播报指定位置词语(SelectedIndex)
        End If
    End Sub
    Private Sub CiYuListView_BeforeLabelEdit(sender As Object, e As LabelEditEventArgs) Handles CiYuListView.BeforeLabelEdit
        移除ToolStripMenuItem.Enabled = False
    End Sub
    Private Sub CiYuListView_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles CiYuListView.AfterLabelEdit
        If e.Label IsNot Nothing Then
            If e.Label.Trim() = "" Then
                e.CancelEdit = True
            End If
        End If
        移除ToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If BoBao.IsZddb() = False Then
            MessageBox.Show("当前不在自动播报！")
            Exit Sub
        End If
        If Button3.Text = "暂停自动播报(&P)" Then
            BoBao.暂停自动播报()
            Label4.Text = "自动播报已暂停"
            Button3.Text = "继续自动播报(&R)"
        Else
            BoBao.继续自动播报()
            Button3.Text = "暂停自动播报(&P)"
        End If
    End Sub

    Private Sub SpVoice1_EndStream(StreamNumber As Integer, StreamPosition As Object) Handles SpVoice1.EndStream
        If ChuFaSpVoice1EndStream = False Then
            ChuFaSpVoice1EndStream = True
            Exit Sub
        End If
        BoBaoWanBi()
    End Sub
    Private Sub BoBaoWanBi()
        BoBao.EndBoBao()
        读选定词语ToolStripMenuItem.Enabled = True
        If BoBao.IsZddb() = False Then
            Label4.Text = "等待播报"
        End If
    End Sub
    ''' <summary>
    ''' 在单词增强目录中寻找音频文件
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAudioFile(ciyu As String) As String
        Dim File As String
        File = ciyu
        File = File.Replace("\", "")
        File = File.Replace("/", "")
        File = File.Replace(":", "")
        File = File.Replace("*", "")
        File = File.Replace("?", "")
        File = File.Replace(Chr(34), "")
        File = File.Replace("<", "")
        File = File.Replace(">", "")
        File = File.Replace("|", "")
        File = CiZuZengQiangMuLu & File & "."
        For Each temp In AudioFileExtension
            If System.IO.File.Exists(File & temp) Then
                Return File & temp
            End If
        Next
        Return Nothing
    End Function
    Private Sub BoBao_BoBao(i As Integer) Handles BoBao.BoBao


        Dim AudioFile As String = GetAudioFile(CiYuListView.Items(i).Text)

        If AudioFile Is Nothing Then
            SpVoice1.Speak(CiYuListView.Items(i).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
        Else
            Try
                If AudioFileStream IsNot Nothing Then
                    AudioFileStream.Dispose()
                    AudioFileStream = Nothing
                End If
                AudioFileStream = New BASS_Stream(AudioFile)
                AudioFileStream.Play()
            Catch ex As Exception
                MessageBox.Show("无法识别文件：" & AudioFile)
                SpVoice1.Speak(CiYuListView.Items(i).Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync)
            End Try
        End If


        Label4.Text = "正在播报第" & (i + 1).ToString() & "个"

        If 自动翻页ToolStripMenuItem.Checked = True Then
            CiYuListView.Items(i).EnsureVisible()
        End If
        If 字词跟随ToolStripMenuItem.Checked = True Then
            For Each ListItem As System.Windows.Forms.ListViewItem In CiYuListView.SelectedItems
                ListItem.Selected = False
            Next
            CiYuListView.Items(i).Selected = True
        End If

        Label1.Text = "本词语播报次数：" + CiYuListView.Items(i).SubItems.Item(1).Text
    End Sub

    ''' <summary>
    ''' 播放完成
    ''' </summary>
    ''' <remarks>该事件不在UI线程触发！</remarks>
    Private Sub AudioFileStream_EndStream() Handles AudioFileStream.EndStream
        If AudioFileStream IsNot Nothing Then
            AudioFileStream.Dispose()
            AudioFileStream = Nothing
        End If
        Me.Invoke(New MethodInvoker(AddressOf BoBaoWanBi))
    End Sub
    Private Sub BoBao_XiaCiBoBaoJianGeBeiGaiBian(i As Integer, jiangge As Integer) Handles BoBao.XiaCiBoBaoJianGeBeiGaiBian
        Label4.Text = jiangge.ToString() & "秒后自动播报第" & i.ToString() & "个"
    End Sub
    Private Sub BoBao_GetCiYu(i As Integer, ByRef retu As String) Handles BoBao.GetCiYu
        retu = CiYuListView.Items(i).Text
    End Sub
    Private Sub BoBao_GetCiYuBoBaoCiShu(i As Integer, ByRef retu As Integer) Handles BoBao.GetCiYuBoBaoCiShu
        retu = Convert.ToInt32(CiYuListView.Items(i).SubItems.Item(1).Text)
    End Sub
    Private Sub BoBao_GetCiYuShuLiang(ByRef retu As Integer) Handles BoBao.GetCiYuShuLiang
        retu = CiYuListView.Items.Count
    End Sub
    Private Sub BoBao_SetCiYuBoBaoCiShu(i As Integer, newValue As Integer) Handles BoBao.SetCiYuBoBaoCiShu
        CiYuListView.Items(i).SubItems.Item(1).Text = newValue.ToString()
    End Sub
    Private Sub BoBao_TingZhiDangQianBoBao() Handles BoBao.TingZhiDangQianBoBao
        If SpVoice1.Status.RunningState <> SpeechLib.SpeechRunState.SRSEDone Then
            ChuFaSpVoice1EndStream = False
            Try
                SpVoice1.Speak(String.Empty, SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak) '部分引擎似乎会出错，所以加个Try
            Catch ex As Exception

            End Try
        Else
            If AudioFileStream IsNot Nothing Then
                AudioFileStream.StopPlay()
                AudioFileStream.Dispose()
                AudioFileStream = Nothing
            End If
        End If
    End Sub
    Private Sub BoBao_ZiDongBoBaoWanCheng(YuanYin As Integer) Handles BoBao.ZiDongBoBaoWanCheng
        Button1.Enabled = True
        Button4.Enabled = True
        Button8.Enabled = True
        Button7.Enabled = True
        读选定词语ToolStripMenuItem.Enabled = True
        Label4.Text = "等待播报"
        If YuanYin = 0 Then
            MessageBox.Show("自动播报已完成！")
        End If
        Button3.Text = "暂停自动播报(&P)"
    End Sub

    Private Sub BoBao_IsZanTing(ByRef retu As Boolean) Handles BoBao.IsZanTing
        retu = (Button3.Text = "继续自动播报(&R)")
    End Sub

    Private Sub 在百度词典中查看ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 在百度词典中查看ToolStripMenuItem.Click
        Dim SelectedItemIndex As Integer
        Dim SelectedItemText As String
        If CiYuListView.SelectedIndices.Count > 0 Then
            SelectedItemIndex = CiYuListView.SelectedIndices.Item(0)
            SelectedItemText = CiYuListView.Items.Item(SelectedItemIndex).Text
            System.Diagnostics.Process.Start("http://dict.baidu.com/s?wd=" & UrlEncode_UTF8(SelectedItemText) & "&ptype=empty")
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

    Private Sub CiYuListView_DragDrop(sender As Object, e As DragEventArgs) Handles CiYuListView.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            OpenFiles(files)
        End If
    End Sub

    Private Sub CiYuListView_DragOver(sender As Object, e As DragEventArgs) Handles CiYuListView.DragOver
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

End Class
