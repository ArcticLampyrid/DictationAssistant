
''' <summary>
''' 播报间隔接口
''' </summary>
''' <remarks></remarks>
''' <summary>
''' Seconds = SecondsPerWord * WordCount + BasedSecond
''' </summary>
Public Class WaitingTimeHelper
    ''' <summary>
    ''' 取单词数（英文）
    ''' </summary>
    ''' <param name="Words">词组</param>
    ''' <returns>单词数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetWordCount(Words As String) As Integer
        Dim Separators As String = ".,:;?!- " '分隔符
        Dim LastSeparator As Integer
        Dim Count As Integer = 1 '数量
        For i As Integer = 0 To Words.Length - 1
            If Separators.IndexOf(Words(i)) <> -1 Then
                If Not LastSeparator = i - 1 Then
                    If Not (LastSeparator = i - 2 AndAlso Words(LastSeparator) = "'"c) Then '缩写视为一个单词
                        Count += 1
                    End If
                End If
                LastSeparator = i
            End If
        Next
        If LastSeparator = Words.Length - 1 Then
            Count -= 1
        End If
        Return Count
    End Function
End Class
