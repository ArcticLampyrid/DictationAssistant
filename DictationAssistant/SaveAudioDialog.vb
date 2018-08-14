Imports System.IO
Imports System.Threading

Public Class SaveAudioDialog
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim channels As Byte = If(ChannelComboBox.SelectedIndex = 0, CByte(1), CByte(2))
        Dim sampleFormat As PcmSampleFormat = If(SampleFormatComboBox.SelectedIndex = 0, PcmSampleFormat.U8, PcmSampleFormat.S16)

        Dim freq As Integer
        Try
            freq = Convert.ToInt32(FreqComboBox.Text)
        Catch ex As Exception
            MessageBox.Show(Me, "采样率必须为纯数字，且不包括Hz等单位符号", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try


        Dim pcmInfo As New PcmFormatInfo(freq, channels, sampleFormat)

        Dim targetFile As String = TargetFileTextBox.Text
        Dim outputFormat As Integer = OutputFormatComboBox.SelectedIndex

        Dim words(Form1.WordListView.Items.Count - 1) As String
        For i = 0 To Form1.WordListView.Items.Count - 1
            words(i) = Form1.WordListView.Items.Item(i).Text
        Next
        Dim engine = CType(Form1.BoBao.Speaker, Speaker).Engine
        Dim rate = CType(Form1.BoBao.Speaker, Speaker).Rate
        Dim speakParam = New SpeakParam() With {.Rate = rate}
        Dim waitingTimeCalculator = Form1.BoBao.CurrentWaitingTimeCalculator
        Dim timesPerWord As Integer = Form1.BoBao.AutoMode_TimesPerWord
        Dim saveLrc As Boolean = CheckBox1.Checked
        Dim writePcmData = Sub(pcmStream As PcmStreamWithInfo)
                               Dim lrcWriter As New LyricWriter
                               Dim lrcFile = Path.ChangeExtension(targetFile, "lrc")
                               Dim byteOffest As Long = 0
                               Using pcmWriterObject = New PcmWriter(pcmStream, True)
                                   For Each word In words
                                       For i = 1 To timesPerWord
                                           lrcWriter.Append(word, pcmWriterObject.BytesToMilliseconds(byteOffest))
                                           Using speech = engine.Speak(word, speakParam)
                                               byteOffest += pcmWriterObject.Write(speech)
                                           End Using
                                           byteOffest += pcmWriterObject.WriteDelay(waitingTimeCalculator.CalculateWaitingTime(word) * 1000)
                                       Next
                                       lrcWriter.Append(" ", pcmWriterObject.BytesToMilliseconds(byteOffest) - 10)
                                   Next
                                   lrcWriter.Append("本文件由 自动默写 程序自动生成", pcmWriterObject.BytesToMilliseconds(byteOffest))
                               End Using
                               If saveLrc Then
                                   Using lrcFileStream = File.Open(lrcFile, FileMode.Create)
                                       lrcWriter.WriteTo(lrcFileStream)
                                   End Using
                               End If
                           End Sub
        Me.Enabled = False

        Timer1.Enabled = True
        Timer1.Tag = 0
        Label6.Text = "已用时0秒"
        Dim t As New Thread(Sub()
                                Try
                                    If outputFormat = 0 Then
                                        Using pcmStream = New PcmStreamWithInfo(File.Open(targetFile, FileMode.Create), pcmInfo)
                                            WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, 0)
                                            Dim headerSize = pcmStream.PcmStream.Length
                                            writePcmData(pcmStream)
                                            pcmStream.PcmStream.Position = 0
                                            WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, pcmStream.PcmStream.Length - headerSize)
                                        End Using
                                    Else
                                        Dim EncoderFileName = "", EncoderArguments = ""
                                        Select Case outputFormat
                                            Case 1
                                                EncoderFileName = "lame.exe"
                                                EncoderArguments = $"--ta 自动默写 -V0 --ignorelength --quiet - ""{targetFile}"""
                                            Case 2
                                                EncoderFileName = "opusenc.exe"
                                                EncoderArguments = $"--artist 自动默写 --ignorelength --quiet - ""{targetFile}"""
                                            Case Else
                                                Throw New Exception("Something impossible happened.")
                                        End Select
                                        Using ProcessObject As New Process()
                                            ProcessObject.StartInfo.FileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase & EncoderFileName
                                            ProcessObject.StartInfo.Arguments = EncoderArguments
                                            ProcessObject.StartInfo.UseShellExecute = False
                                            ProcessObject.StartInfo.CreateNoWindow = True
                                            ProcessObject.StartInfo.RedirectStandardInput = True
                                            ProcessObject.Start()
                                            Using pcmStream = New PcmStreamWithInfo(ProcessObject.StandardInput.BaseStream, pcmInfo)
                                                WriteWaveHeader(pcmStream.PcmStream, pcmStream.Format, 0)
                                                writePcmData(pcmStream)
                                            End Using
                                            ProcessObject.WaitForExit()
                                            If ProcessObject.ExitCode <> 0 Then
                                                Throw New Exception($"编码错误：{ProcessObject.ExitCode}，编码器：{ProcessObject.StartInfo.FileName}，参数：{ProcessObject.StartInfo.Arguments}")
                                            End If
                                        End Using
                                    End If


                                    Me.Invoke(Sub()
                                                  Timer1.Enabled = False
                                                  Me.Enabled = True
                                                  MessageBox.Show(Me, "Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                              End Sub)
                                Catch ex As Exception
                                    Timer1.Enabled = False
                                    Me.Invoke(Sub()
                                                  Timer1.Enabled = False
                                                  Me.Enabled = True
                                                  MessageBox.Show(Me, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                              End Sub)
                                End Try
                            End Sub)
        t.Start()
    End Sub

    Private Sub SaveAudioDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChannelComboBox.SelectedIndex = 1
        SampleFormatComboBox.SelectedIndex = 1
        OutputFormatComboBox.SelectedIndex = 1
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Select Case OutputFormatComboBox.SelectedIndex
            Case 0
                SaveFileDialog1.FilterIndex = 1
            Case 1
                SaveFileDialog1.FilterIndex = 2
            Case 2
                SaveFileDialog1.FilterIndex = 3
        End Select
        SaveFileDialog1.FileName = TargetFileTextBox.Text
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Select Case SaveFileDialog1.FilterIndex
                Case 1
                    OutputFormatComboBox.SelectedIndex = 0
                Case 2
                    OutputFormatComboBox.SelectedIndex = 1
                Case 3
                    OutputFormatComboBox.SelectedIndex = 2
            End Select
            TargetFileTextBox.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub OutputFormatComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OutputFormatComboBox.SelectedIndexChanged

        Select Case OutputFormatComboBox.SelectedIndex
            Case 0
                TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".wav")
            Case 1
                TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".mp3")
            Case 2
                TargetFileTextBox.Text = Path.ChangeExtension(TargetFileTextBox.Text, ".opus")
        End Select
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim second = CType(Timer1.Tag, Integer)
        second += 1
        Timer1.Tag = second
        Label6.Text = $"已用时{second}秒"
    End Sub
End Class