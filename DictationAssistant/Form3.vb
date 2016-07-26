Public Class Form3
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rand = New Random()

        Dim sjxcsl As Integer
        sjxcsl = Convert.ToInt32(NumericUpDown1.Value)
        Do Until sjxcsl = Form1.CiYuListView.Items.Count
            Form1.CiYuListView.Items(rand.Next(0, Form1.CiYuListView.Items.Count)).Remove() '随机删除一个词语
        Loop
        Me.Close()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumericUpDown1.Maximum = Form1.CiYuListView.Items.Count
    End Sub
End Class