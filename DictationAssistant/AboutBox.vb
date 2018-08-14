Public NotInheritable Class AboutBox

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Text = "关于 " & My.Application.Info.Title
        LabelProductName.Text = My.Application.Info.ProductName
        LabelVersion.Text = "版本 " & My.Application.Info.Version.ToString
        TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Close()
    End Sub

End Class
