Module Module1
    Public CiZuZengQiangMuLu As String

    Public Function GetChineseEngineLevel(Name As String) As Integer
        Select Case Name
            Case "VW Lily"
                Return 5
            Case "VW Hui"
                Return 4
            Case "VW Liang"
                Return 3
            Case "VW Wang"
                Return 2
            Case "Girl XiaoKun"
                Return 1
            Case Else
                Return 0
        End Select
    End Function
    Public Function GetEnglishEngineLevel(Name As String) As Integer
        Select Case Name
            Case "Microsoft Hazel Desktop"
                Return 4
            Case "VW Julie"
                Return 3
            Case "VW Paul"
                Return 2
            Case "VW Kate"
                Return 1
            Case Else
                Return 0
        End Select
    End Function
End Module
