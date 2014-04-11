Public Class Form3

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sjxcsl As Integer
        sjxcsl = Convert.ToInt32(NumericUpDown1.Value)
        Do Until sjxcsl = Form1.ListView1.Items.Count
            Dim rand = New System.Random()
            Dim yscdcysy As Integer '要删除的词语索引
            yscdcysy = rand.Next(0, Form1.ListView1.Items.Count)
            Form1.ListView1.Items(yscdcysy).Remove()
        Loop
        Me.Close()
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumericUpDown1.Maximum = Form1.ListView1.Items.Count
    End Sub
End Class