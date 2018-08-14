Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class PcmPlayer
    Implements IDisposable
    Public Enum AudioPlayState
        Stopped = 0
        Playing = 1
        Paused = 2
    End Enum
#Region "SDL2库函数定义"
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_OpenAudioDevice(ByVal device As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByVal iscapture As Boolean, ByRef desired As SDL_AudioSpec, ByRef obtained As SDL_AudioSpec, ByVal allowed_changes As Integer) As UInteger
    End Function
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub SDL_PauseAudioDevice(ByVal dev As UInteger, <MarshalAs(UnmanagedType.Bool)> ByVal pause_on As Boolean)
    End Sub
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub SDL_CloseAudioDevice(ByVal dev As UInteger)
    End Sub
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub SDL_LockAudioDevice(ByVal dev As UInteger)
    End Sub
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub SDL_UnlockAudioDevice(ByVal dev As UInteger)
    End Sub
    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_GetAudioDeviceStatus(ByVal dev As UInteger) As Integer
    End Function

    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_MixAudioFormat(ByVal dst As IntPtr,
                                               <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=3)> src As Byte(),
                                               ByVal format As UShort,
                                               ByVal len As Integer,
                                               ByVal volume As Integer) As Integer
    End Function
    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Private Delegate Sub SDL_AudioCallback(ByVal userdata As IntPtr, ByVal stream As IntPtr, ByVal len As Integer)

    <StructLayout(LayoutKind.Sequential)>
    Private Structure SDL_AudioSpec
        Public freq As Integer
        Public format As UShort
        Public channels As Byte
        Public silence As Byte
        Public samples As UShort
        Public padding As UShort
        Public size As UInteger
        Public callback As SDL_AudioCallback
        Public userdata As IntPtr
    End Structure
#End Region

    Shared Sub New()
        SdlAudio.Use()
    End Sub

    Public ReadOnly Property State As AudioPlayState
        Get
            If AudioDeviceID = 0 Then
                Return AudioPlayState.Stopped
            End If
            Select Case SDL_GetAudioDeviceStatus(AudioDeviceID)
                Case 0
                    Return AudioPlayState.Stopped
                Case 1
                    Return AudioPlayState.Playing
                Case 2
                    Return AudioPlayState.Paused
                Case Else 'unknown
                    Return AudioPlayState.Stopped
            End Select
        End Get
    End Property

    Private PcmStream As Stream
    Private AudioDeviceID As UInteger
    Private gc_handle As GCHandle
    Private SdlVolume As Byte
    Private PlayCompletedEventHandlerList As New List(Of EventHandler)
    Public Event PlayCompleted As EventHandler
    Private audioSpec As SDL_AudioSpec
    Private Format As PcmSampleFormat
    Private Sub Callback_fill(ByVal userdata As IntPtr, ByVal stream As IntPtr, ByVal len As Integer)
        If len = 0 OrElse stream = IntPtr.Zero Then
            Return
        End If

        Dim data(len - 1) As Byte
        PcmSampleFormatHelper.SetAsEmptyAudio(Format, data)
        Marshal.Copy(data, 0, stream, len)
        If PcmStream IsNot Nothing Then
            Dim numOfRead As Integer = 0
            While numOfRead < len
                Dim t = PcmStream.Read(data, numOfRead, len - numOfRead)
                numOfRead += t
                If t = 0 Then
                    Exit While
                End If
            End While

            If numOfRead = 0 Then
                PcmStream.Dispose()
                PcmStream = Nothing

                Dim t As New Thread(Sub()
                                        Dispose()
                                        RaiseEvent PlayCompleted(Me, EventArgs.Empty)
                                    End Sub)
                t.Start()
            Else
                SDL_MixAudioFormat(stream, data, Format, numOfRead, SdlVolume)
            End If
        End If
    End Sub

    Public Sub New(pcm As PcmStreamWithInfo, Optional volume As Byte = 100)
        PcmStream = pcm.PcmStream
        SdlVolume = CType(volume * 128 \ 100, Byte)
        Format = pcm.Format.SampleFormat
        audioSpec = New SDL_AudioSpec With {
            .freq = pcm.Format.Freq,
            .format = Format,
            .channels = pcm.Format.Channels,
            .samples = 1024,
            .callback = AddressOf Callback_fill,
            .userdata = IntPtr.Zero
        }
        AudioDeviceID = SDL_OpenAudioDevice(IntPtr.Zero, False, audioSpec, Nothing, 0)
    End Sub
    Public Sub Play()
        If Not gc_handle.IsAllocated Then
            gc_handle = GCHandle.Alloc(Me, GCHandleType.Normal)
        End If
        SDL_PauseAudioDevice(AudioDeviceID, False)
    End Sub
    Public Sub Pause()
        SDL_LockAudioDevice(AudioDeviceID)
        If AudioDeviceID <> 0 Then
            SDL_PauseAudioDevice(AudioDeviceID, True)
            SDL_UnlockAudioDevice(AudioDeviceID)
        End If
        If gc_handle.IsAllocated Then
            gc_handle.Free()
        End If
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 要检测冗余调用

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If AudioDeviceID <> 0 Then
                SDL_LockAudioDevice(AudioDeviceID)
                If AudioDeviceID <> 0 Then
                    SDL_CloseAudioDevice(AudioDeviceID)
                    SDL_UnlockAudioDevice(AudioDeviceID)
                    AudioDeviceID = 0
                End If
            End If

            If disposing Then
                If PcmStream IsNot Nothing Then
                    PcmStream.Dispose()
                End If
                If gc_handle.IsAllocated Then
                    gc_handle.Free()
                End If
            End If

            PcmStream = Nothing
        End If
        disposedValue = True
    End Sub


    Protected Overrides Sub Finalize()
        ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        Dispose(False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
