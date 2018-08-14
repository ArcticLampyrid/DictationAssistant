Imports 自动默写

Public Class Speaker
    Implements ISpeaker
    Public Property Volume As Byte
    Public Property Rate As SByte
    Public Property Engine As ISpeakEngine
    Function Speak(Text As String) As ISpeakStateControler Implements ISpeaker.Speak
        Dim pcm = Engine.Speak(Text, New SpeakParam() With {.Rate = Rate})
        Dim player = New PcmPlayer(pcm, Volume)
        player.Play()
        Return New SpeakStateControler(player)
    End Function
	
    Private Class SpeakStateControler
        Implements ISpeakStateControler
        Private lock As New Object
        Private WithEvents PcmPlayer As PcmPlayer
        Public Sub New(PcmPlayer As PcmPlayer)
            Me.PcmPlayer = PcmPlayer
        End Sub

        Private PlayCompletedEventHandlerList As New List(Of EventHandler)
        Public Custom Event PlayCompleted As EventHandler Implements ISpeakStateControler.PlayCompleted
            AddHandler(ByVal value As EventHandler)
                SyncLock lock
                    PlayCompletedEventHandlerList.Add(value)
                End SyncLock
                If PcmPlayer.State = PcmPlayer.AudioPlayState.Stopped Then
                    value.Invoke(Me, EventArgs.Empty)
                End If
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                SyncLock lock
                    PlayCompletedEventHandlerList.Remove(value)
                End SyncLock
            End RemoveHandler
            RaiseEvent()
                Dim TempList As EventHandler()
                SyncLock lock
                    ReDim TempList(PlayCompletedEventHandlerList.Count - 1)
                    PlayCompletedEventHandlerList.CopyTo(TempList)
                End SyncLock
                For Each handler As EventHandler In TempList
                    handler.Invoke(Me, EventArgs.Empty)
                Next
            End RaiseEvent
        End Event
        Private Sub PcmPlayer_PlayCompleted(sender As Object, e As EventArgs) Handles PcmPlayer.PlayCompleted
            RaiseEvent PlayCompleted()
        End Sub
        Public Sub StopSpeak() Implements ISpeakStateControler.StopSpeak
            PcmPlayer.Dispose()
        End Sub
    End Class
End Class

