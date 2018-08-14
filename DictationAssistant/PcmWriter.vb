Imports System.IO
Imports System.Runtime.InteropServices

Public Class PcmWriter
    Implements IDisposable

    Shared Sub New()
        SdlAudio.Use()
    End Sub
    <StructLayout(LayoutKind.Sequential)>
    Private Structure SDL_AudioCVT
        <MarshalAs(UnmanagedType.Bool)>
        Public needed As Boolean
        Public src_format As UShort
        Public dst_format As UShort
        Public rate_incr As Double
        Public buf As IntPtr
        Public len As Integer
        Public len_cvt As Integer
        Public len_mult As Integer
        Public len_ratio As Double
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=10)>
        Public filters As IntPtr()
        Public filter_index As Integer
    End Structure

    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_BuildAudioCVT(ByRef cvt As SDL_AudioCVT, ByVal src_format As UShort, ByVal src_channels As Byte, ByVal src_rate As Integer, ByVal dst_format As UShort, ByVal dst_channels As Byte, ByVal dst_rate As Integer) As Integer
    End Function

    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_ConvertAudio(ByRef cvt As SDL_AudioCVT) As Integer
    End Function

    Private OutStream As Stream
    Private FormatInfo As PcmFormatInfo
    Private _leaveOpen As Boolean

    Public Sub New(output As PcmStreamWithInfo, Optional leaveOpen As Boolean = False)
        OutStream = output.PcmStream
        FormatInfo = output.Format
        _leaveOpen = leaveOpen
    End Sub
    Public Function MillisecondsToSamples(ms As Long) As Long
        Return ((ms \ 1000) * FormatInfo.Freq) + (((ms Mod 1000) * FormatInfo.Freq) \ 1000)
    End Function
    Public Function MillisecondsToBytes(ms As Long) As Long
        Return MillisecondsToSamples(ms) * FormatInfo.BlockAlign
    End Function

    Friend Function BytesToMilliseconds(byteOffest As Long) As Long
        Return SamplesToMilliseconds(byteOffest \ FormatInfo.BlockAlign)
    End Function
    Friend Function SamplesToMilliseconds(samples As Long) As Long
        Return (samples \ FormatInfo.Freq) * 1000 + ((samples Mod FormatInfo.Freq) * 1000) \ FormatInfo.Freq
    End Function
    Public Function WriteDelay(ms As Long) As Long
        Dim len = Convert.ToInt32(MillisecondsToBytes(ms))
        Dim data(len - 1) As Byte
        PcmSampleFormatHelper.SetAsEmptyAudio(FormatInfo.SampleFormat, data)
        OutStream.Write(data, 0, len)
        Return len
    End Function
    Public Function Write(input As PcmStreamWithInfo) As Long
        Dim cvt As New SDL_AudioCVT
        ReDim cvt.filters(9)
        SDL_BuildAudioCVT(cvt,
                          input.Format.SampleFormat, input.Format.Channels, input.Format.Freq,
                          FormatInfo.SampleFormat, FormatInfo.Channels, FormatInfo.Freq)
        Dim len = 1024 * input.Format.BlockAlign
        Dim buf(len * cvt.len_mult - 1) As Byte
        Dim p As GCHandle = GCHandle.Alloc(buf, GCHandleType.Pinned)
        cvt.buf = p.AddrOfPinnedObject()
        Dim totalBytes As Long = 0
        Do
            Dim numOfRead As Integer = 0
            While numOfRead < len
                Dim t = input.PcmStream.Read(buf, numOfRead, len - numOfRead)
                numOfRead += t
                If t = 0 Then
                    Exit While
                End If
            End While
            If numOfRead = 0 Then
                Exit Do
            End If
            cvt.len = numOfRead
            SDL_ConvertAudio(cvt)
            OutStream.Write(buf, 0, cvt.len_cvt)
            totalBytes += cvt.len_cvt
        Loop
        p.Free()
        p = Nothing
        If (totalBytes Mod FormatInfo.BlockAlign <> 0) Then
            Dim lenAlign As Integer = Convert.ToInt32(FormatInfo.BlockAlign - (totalBytes Mod FormatInfo.BlockAlign))
            Dim bufAlign(lenAlign - 1) As Byte
            OutStream.Write(bufAlign, 0, lenAlign)
            totalBytes += lenAlign
        End If
        Return totalBytes
    End Function

#Region "IDisposable Support"
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposing Then
            If _leaveOpen Then
                OutStream.Flush()
            Else
                OutStream.Close()
            End If
        End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region
End Class
