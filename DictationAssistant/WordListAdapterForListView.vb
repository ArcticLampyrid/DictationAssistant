Imports 自动默写

Public Class WordListAdapterForListView
    Implements IWordListAdapter
    Dim ListViewObject As ListView
    Public Sub New(ListViewObject As ListView)
        Me.ListViewObject = ListViewObject
    End Sub

    Default Public ReadOnly Property Item(index As Integer) As String Implements IWordListAdapter.Item
        Get
            Return CType(ListViewObject.Invoke(Function() ListViewObject.Items(index).Text), String)
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IWordListAdapter.Count
        Get
            Return CType(ListViewObject.Invoke(Function() ListViewObject.Items.Count), Integer)
        End Get
    End Property
End Class
