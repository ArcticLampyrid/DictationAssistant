Public Class Form2

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DCZQML = "" Then
            CheckBox1.Checked = False
        Else
            CheckBox1.Checked = True
        End If
        If Strings.Right(DCZQML, 1) = "\" And Len(DCZQML) > 3 Then
            TextBox1.Text = Mid(DCZQML, 1, Len(DCZQML) - 1)
        Else
            TextBox1.Text = DCZQML
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CheckBox1.Checked = False Then
            DCZQML = ""
            Me.Close()
            SaveSetting("自动默写", "TTS引擎", "单词增强", DCZQML)
        Else
            If System.IO.Directory.Exists(TextBox1.Text) = False Then
                MsgBox("不存在该文件夹！")
            Else
                DCZQML = TextBox1.Text
                If Strings.Right(DCZQML, 1) <> "\" Then
                    DCZQML = DCZQML & "\"
                End If
                SaveSetting("自动默写", "TTS引擎", "单词增强", DCZQML)
                Me.Close()
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
    End Sub
End Class