Imports System.IO
Imports System.Runtime.InteropServices
Imports 自动默写

Public NotInheritable Class AudioFileDecodeStream
    Inherits Stream
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_StreamCreateFile(<MarshalAs(UnmanagedType.Bool)> ByVal mem As Boolean, ByVal file As String, ByVal offset As Long, ByVal length As Long, ByVal flags As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_StreamFree(ByVal handle As Integer) As Integer
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
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelGetInfo(ByVal handle As Integer, ByRef info As BASS_CHANNELINFO) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelGetData(ByVal handle As Integer, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=2)> buffer As Byte(), length As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelGetData(ByVal handle As Integer, buffer As IntPtr, length As Integer) As Integer
    End Function
    <DllImport("bass.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Private Shared Function BASS_ChannelGetLength(ByVal handle As Integer, ByVal mode As Integer) As Long
    End Function
    <StructLayout(LayoutKind.Sequential)>
    Public Structure BASS_CHANNELINFO
        Public freq As Integer
        Public chans As Integer
        Public flags As Integer
        Public channelType As Integer
        Public origres As Integer
        Public plugin As Integer
        Public sample As Integer
        Public filename As Integer
    End Structure
    Private Const BASS_UNICODE As Integer = &H80000000
    Private Const BASS_STREAM_DECODE As Integer = &H200000
    Private Const BASS_SAMPLE_8BITS As Integer = 1
    Private Const BASS_POS_BYTE As Integer = 0

    Public Shared Function Create(url As String) As PcmStreamWithInfo
        Dim PcmStream As New AudioFileDecodeStream(url)
        Return New PcmStreamWithInfo(PcmStream, PcmStream.Format)
    End Function


    Private chan As Integer

    Public ReadOnly Property Format As PcmFormatInfo
    Private Sub New(url As String)
        chan = BASS_StreamCreateFile(False, url, 0L, 0L, BASS_STREAM_DECODE Or BASS_UNICODE)
        If chan = 0 Then
            Throw New Exception()
        End If

        Dim info As New BASS_CHANNELINFO
        BASS_ChannelGetInfo(chan, info)

        Format = New PcmFormatInfo(info.freq,
                                       CType(info.chans, Byte),
                                       If((info.flags And BASS_SAMPLE_8BITS) <> 0, PcmSampleFormat.U8, PcmSampleFormat.S16))
    End Sub


    Public Overrides ReadOnly Property CanWrite As Boolean = False
    Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
        Throw New NotSupportedException()
    End Sub
    Public Overrides Sub Flush()

    End Sub
    Public Overrides ReadOnly Property CanSeek As Boolean = False
    Public Overrides ReadOnly Property Length As Long
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Public Overrides Property Position As Long
        Get
            Throw New NotSupportedException()
        End Get
        Set(value As Long)
            Throw New NotSupportedException()
        End Set
    End Property
    Public Overrides Sub SetLength(value As Long)
        Throw New NotSupportedException()
    End Sub
    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
        Throw New NotSupportedException()
    End Function
    Public Overrides ReadOnly Property CanRead As Boolean = True
    Private _position As Integer = 0
    Private buffer_bass_startPosition As Integer = 0
    Private buffer_bass(512 - 1) As Byte
    Private buffer_bass_length As Integer = 0
    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
        If chan = 0 Then
            Return 0
        End If
        Dim outputLength As Integer
        Dim numOfNeed As Integer
        numOfNeed = count
        While numOfNeed > 0
            If numOfNeed > 0 Then
                Dim buffer_bass_numOfUnread As Integer = buffer_bass_length - buffer_bass_startPosition
                If buffer_bass_numOfUnread > 0 Then
                    If buffer_bass_numOfUnread > numOfNeed Then
                        System.Array.Copy(buffer_bass, buffer_bass_startPosition, buffer, offset + outputLength, numOfNeed)
                        _position += numOfNeed
                        buffer_bass_startPosition += numOfNeed

                        outputLength += numOfNeed
                        numOfNeed = 0
                    Else
                        System.Array.Copy(buffer_bass, buffer_bass_startPosition, buffer, offset + outputLength, buffer_bass_numOfUnread)
                        _position += buffer_bass_numOfUnread
                        buffer_bass_startPosition = 0
                        buffer_bass_length = 0

                        outputLength += buffer_bass_numOfUnread
                        numOfNeed -= buffer_bass_numOfUnread
                    End If
                ElseIf numOfNeed > buffer_bass.Length Then '大数据直接读
                    Dim p As GCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
                    Dim numOfOutput = BASS_ChannelGetData(chan, CType(p.AddrOfPinnedObject().ToInt64() + (offset + outputLength), IntPtr), numOfNeed)
                    p.Free()
                    p = Nothing


                    If numOfOutput > 0 Then
                        _position += numOfOutput

                        outputLength += numOfOutput
                        numOfNeed -= numOfOutput
                    Else
                        Exit While
                    End If
                Else '小段数据启用缓存（同时防止读取长度不到一块时读不出的问题）
                    buffer_bass_startPosition = 0
                    buffer_bass_length = BASS_ChannelGetData(chan, buffer_bass, buffer_bass.Length)
                    If buffer_bass_length < 0 Then
                        buffer_bass_length = 0
                        Exit While
                    End If
                End If
            End If
        End While
        Return outputLength
    End Function

    Public Overrides Sub Close()
        MyBase.Close()
        If chan <> 0 Then
            BASS_StreamFree(chan)
            chan = 0
        End If
    End Sub
End Class
