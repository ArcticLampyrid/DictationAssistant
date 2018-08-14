Imports System.IO
Imports System.Text

Public Class LyricWriter
    Private Lyric As New Dictionary(Of String, Dictionary(Of Long, Object))
    Public ReadOnly IdTag As New Dictionary(Of String, String)
    Public Sub Append(text As String, offest As Long)
        If Not Lyric.ContainsKey(text) Then
            Lyric.Add(text, New Dictionary(Of Long, Object))
        End If
        Lyric.Item(text).Add(offest, Nothing)
    End Sub
    Public Sub WriteTo(out As Stream)
        Using writer As New StreamWriter(out, Encoding.Default)
            For Each pair In IdTag
                writer.WriteLine($"[{pair.Key}:{pair.Value}]")
            Next
            For Each pair In Lyric
                For Each offest In pair.Value.Keys
                    Dim min = offest \ 60000
                    Dim sec = (offest Mod 60000) / 1000
                    writer.Write($"[{min.ToString("D2")}:{sec.ToString("00.00")}]")
                Next
                writer.WriteLine(pair.Key)
            Next
        End Using
    End Sub
End Class
