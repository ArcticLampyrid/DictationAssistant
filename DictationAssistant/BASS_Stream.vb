Public Class BASS_Stream
    ''' <summary>
    ''' 播放完成
    ''' </summary>
    ''' <remarks>该事件不在UI线程触发！</remarks>
    Event EndStream()

    Private Declare Unicode Function BASS_StreamCreateFile Lib "bass.dll" (ByVal mem As Int32, ByVal file As String, ByVal offset As Int64, ByVal length As Int64, ByVal flags As Int32) As Int32
    Private Declare Unicode Function BASS_StreamFree Lib "bass.dll" (ByVal handle As Int32) As Int32
    Private Declare Unicode Function BASS_ChannelSetSync Lib "bass.dll" (ByVal handle As Int32, ByVal type As Int32, ByVal param As Int64, ByVal proc As SyncProc, ByVal user As IntPtr) As Int32
    Private Declare Unicode Function BASS_ChannelPlay Lib "bass.dll" (ByVal handle As Int32, ByVal restart As Int32) As Int32
    Private Declare Unicode Function BASS_ChannelStop Lib "bass.dll" (ByVal handle As Int32) As Int32

    Private Declare Unicode Function BASS_ChannelIsActive Lib "bass.dll" (ByVal handle As Int32) As Int32

    Private Const BASS_ACTIVE_STOPPED As Int32 = 0

    Private Const BASS_UNICODE As Int32 = &H80000000
    Private Const BASS_SYNC_END As Int32 = 2
    Dim HStream As Int32
    Private Delegate Sub SyncProc(ByVal handle As Int32, ByVal channel As Int32, ByVal data As Int32, ByVal user As IntPtr)

    Private Callback_EndSyncProc As SyncProc = AddressOf Me.Callback_End '一定要注意本变量的生命周期，销毁后回调会出错

    Public Sub New(file As String)
        HStream = BASS_StreamCreateFile(0, file, 0L, 0L, BASS_UNICODE)
        BASS_ChannelSetSync(HStream, BASS_SYNC_END, 0, Callback_EndSyncProc, New IntPtr(0))
        If HStream = 0 Then
            Throw New Exception()
        End If
    End Sub

    Private Sub Callback_End(ByVal handle As Int32, ByVal channel As Int32, ByVal data As Int32, ByVal user As IntPtr)
        RaiseEvent EndStream()
    End Sub

    Public Sub Play()
        Play(True)
    End Sub
    Public Sub Play(ByVal restart As Boolean)
        If restart Then
            BASS_ChannelPlay(HStream, 1)
        Else
            BASS_ChannelPlay(HStream, 0)
        End If
    End Sub
    ''' <summary>
    ''' 并不会触发EndStream事件
    ''' </summary>
    Public Sub StopPlay()
        BASS_ChannelStop(HStream)
    End Sub
    Public Sub Dispose()
        BASS_StreamFree(HStream)
    End Sub
    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
    Public ReadOnly Property IsStopped As Boolean
        Get
            Return BASS_ChannelIsActive(HStream) = BASS_ACTIVE_STOPPED
        End Get
    End Property
End Class
