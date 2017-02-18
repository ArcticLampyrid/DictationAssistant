Imports System.Runtime.InteropServices

Public NotInheritable Class BASS_Stream
    ''' <summary>
    ''' 播放完成
    ''' </summary>
    ''' <remarks>该事件不在UI线程触发！</remarks>
    Event EndStream()

    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_StreamCreateFile(ByVal mem As Integer, ByVal file As String, ByVal offset As Long, ByVal length As Long, ByVal flags As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_StreamFree(ByVal handle As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelSetSync(ByVal handle As Integer, ByVal type As Integer, ByVal param As Long, ByVal proc As SyncProc, ByVal user As IntPtr) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelPlay(ByVal handle As Integer, ByVal restart As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelStop(ByVal handle As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelIsActive(ByVal handle As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelSetAttribute(ByVal handle As Integer, ByVal attrib As Integer, ByVal value As Single) As Integer
    End Function

    Private Const BASS_ACTIVE_STOPPED As Integer = 0
    Private Const BASS_ATTRIB_VOL As Integer = 2
    Private Const BASS_UNICODE As Integer = &H80000000
    Private Const BASS_SYNC_END As Integer = 2

    Private Delegate Sub SyncProc(ByVal handle As Integer, ByVal channel As Integer, ByVal data As Integer, ByVal user As IntPtr)

    Private Shared Callback_EndSyncProc As SyncProc = AddressOf Callback_End '一定要注意本变量的生命周期，销毁后回调会出错

    Private Shared HandleToObject As New Dictionary(Of Integer, BASS_Stream)

    Private Shared Sub Callback_End(ByVal handle As Integer, ByVal channel As Integer, ByVal data As Integer, ByVal user As IntPtr)
        Dim s As BASS_Stream
        Try
            SyncLock HandleToObject
                s = HandleToObject.Item(CInt(user))
            End SyncLock
        Catch ex As KeyNotFoundException
            BASS_StreamFree(CInt(user))
            Return
        End Try
        s.OnEndStream()
    End Sub

    Dim HStream As Integer
    Private Sub OnEndStream()
        StopPlay()
        RaiseEvent EndStream()
    End Sub
    Public Sub New(file As String, volume As Single)
        HStream = BASS_StreamCreateFile(0, file, 0L, 0L, BASS_UNICODE)
        If HStream = 0 Then
            Throw New Exception()
        End If
        BASS_ChannelSetSync(HStream, BASS_SYNC_END, 0, Callback_EndSyncProc, CType(HStream, IntPtr))
        SetVolume(volume)
        SyncLock HandleToObject
            HandleToObject.Add(HStream, Me)
        End SyncLock
        BASS_ChannelPlay(HStream, 1)
    End Sub


    ''' <summary>
    ''' 并不会触发EndStream事件
    ''' </summary>
    Public Sub StopPlay()
        If HStream <> 0 Then
            SyncLock HandleToObject
                HandleToObject.Remove(HStream)
            End SyncLock
            BASS_StreamFree(HStream)
            HStream = 0
        End If
    End Sub
    ''' <summary>
    ''' 设置音量
    ''' </summary>
    ''' <param name="volume">音量等级，0（无声）~1（最大）</param>
    Public Sub SetVolume(volume As Single)
        If HStream = 0 Then
            Return
        End If
        BASS_ChannelSetAttribute(HStream, BASS_ATTRIB_VOL, volume)
    End Sub

    Public ReadOnly Property IsStopped As Boolean
        Get
            Return HStream = 0
        End Get
    End Property
End Class
