Public Class SetImprovedSpeakEngineDialog

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
        TextBox1.Text = CiZuZengQiangMuLu
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not CheckBox1.Checked Then
            CiZuZengQiangMuLu = ""
            SaveSetting("自动默写", "选项", "词组增强", CiZuZengQiangMuLu)
        Else
            If IO.Directory.Exists(TextBox1.Text) = False Then
                MessageBox.Show("不存在该文件夹！")
            Else
                CiZuZengQiangMuLu = TextBox1.Text
                SaveSetting("自动默写", "选项", "词组增强", CiZuZengQiangMuLu)
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