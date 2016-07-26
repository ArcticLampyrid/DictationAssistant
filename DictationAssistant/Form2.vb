Public Class Form2

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CiZuZengQiangMuLu = "" Then
            CheckBox1.Checked = False
        Else
            CheckBox1.Checked = True
        End If
        If Strings.Right(CiZuZengQiangMuLu, 1) = "\" And Len(CiZuZengQiangMuLu) > 3 Then
            TextBox1.Text = Mid(CiZuZengQiangMuLu, 1, Len(CiZuZengQiangMuLu) - 1)
        Else
            TextBox1.Text = CiZuZengQiangMuLu
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CheckBox1.Checked = False Then
            CiZuZengQiangMuLu = ""
            SaveSetting("自动默写", "选项", "词组增强", CiZuZengQiangMuLu)
            Me.Close()
        Else
            If System.IO.Directory.Exists(TextBox1.Text) = False Then
                MsgBox("不存在该文件夹！")
            Else
                CiZuZengQiangMuLu = TextBox1.Text
                If Strings.Right(CiZuZengQiangMuLu, 1) <> "\" Then
                    CiZuZengQiangMuLu = CiZuZengQiangMuLu & "\"
                End If
                SaveSetting("自动默写", "选项", "词组增强", CiZuZengQiangMuLu)
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