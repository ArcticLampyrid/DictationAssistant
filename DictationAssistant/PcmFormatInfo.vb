Imports System.IO

Public Class PcmFormatInfo
    Public Sub New(freq As Integer, channels As Byte, sampleFormat As PcmSampleFormat)
        Me.Freq = freq
        Me.Channels = channels
        Me.SampleFormat = sampleFormat
    End Sub

    Public ReadOnly Property Freq As Integer
    Public ReadOnly Property Channels As Byte
    Public ReadOnly Property SampleFormat As PcmSampleFormat

    Public ReadOnly Property BlockAlign As UShort
        Get
            Return Channels * PcmSampleFormatHelper.GetByteSize(SampleFormat)
        End Get
    End Property
End Class

Public NotInheritable Class PcmStreamWithInfo
    Implements IDisposable
    Public Shared Empty As New PcmStreamWithInfo(Stream.Null, New PcmFormatInfo(44100, 2, PcmSampleFormat.S16))
    Public ReadOnly Property PcmStream As Stream
    Public ReadOnly Property Format As PcmFormatInfo
    Public Sub New(PcmStream As Stream, Format As PcmFormatInfo)
        Me.PcmStream = PcmStream
        Me.Format = Format
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        PcmStream.Dispose()
    End Sub
End Class

Public Enum PcmSampleFormat As UShort 'SDL兼容
    U8 = &H8
    S16 = &H8010
End Enum
Public Class PcmSampleFormatHelper
    Public Shared Function GetBitSize(a As PcmSampleFormat) As UShort
        Return a And &HFFUS
    End Function
    Public Shared Function GetByteSize(a As PcmSampleFormat) As UShort
        Return (a And &HFFUS) >> 3
    End Function
    Public Shared Function IsFloat(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 8)) <> 0
    End Function
    Public Shared Function IsBigEndian(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 12)) <> 0
    End Function
    Public Shared Function IsSigned(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 15)) <> 0
    End Function
    Public Shared Function IsInt(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 8)) = 0
    End Function
    Public Shared Function IsLittleEndian(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 12)) = 0
    End Function
    Public Shared Function IsUnsigned(a As PcmSampleFormat) As Boolean
        Return (a And (1US << 15)) = 0
    End Function
    Public Shared Sub SetAsEmptyAudio(a As PcmSampleFormat, data As Byte())
        For i = 0 To data.Length - 1
            data(i) = 0
            If IsUnsigned(a) AndAlso i Mod GetByteSize(a) = 0 Then
                data(i) = &H80
            End If
        Next
    End Sub

    Public Shared Function IsEmptyAudio(a As PcmSampleFormat, data As Byte(), start As Integer, length As Integer) As Boolean
        For i = start To start + length - 1
            If IsUnsigned(a) AndAlso (i - start) Mod GetByteSize(a) = 0 Then
                If data(i) <> &H80 Then
                    Return False
                End If
            Else
                If data(i) <> 0 Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function
End Class

Module PcmAudioHelper
    Public Sub WriteWaveHeader(output As Stream, format As PcmFormatInfo, dataLength As Long)
        If (format.SampleFormat <> PcmSampleFormat.U8 AndAlso format.SampleFormat <> PcmSampleFormat.S16) Then
            Throw New NotSupportedException
        End If
        Dim writer As New BinaryWriter(output)
        Dim wBitsPerSample As UShort = PcmSampleFormatHelper.GetBitSize(format.SampleFormat)
        Dim nBlockAlign As UShort = format.BlockAlign
        writer.Write(&H46464952) 'Ascii"RIFF"
        writer.Write(CType(dataLength + 36, UInteger))
        writer.Write(&H45564157) 'Ascii"WAVE"
        writer.Write(&H20746D66) 'Ascii"fmt "
        writer.Write(&H10)
        writer.Write(1S) 'wFormatTag 1=线性PCM编码
        writer.Write(CType(format.Channels, Short)) 'nChannels 通道数
        writer.Write(CType(format.Freq, Integer)) 'nSamplesPerSec 采样频率
        writer.Write(CType(format.Freq * nBlockAlign, Integer)) 'nAvgBytesPerSec 每秒平均字节数
        writer.Write(CType(nBlockAlign, UShort)) 'nBlockAlign 数据块长度
        writer.Write(CType(wBitsPerSample, UShort)) 'wBitsPerSample PCM位宽
        writer.Write(&H61746164) 'Ascii"data"
        writer.Write(CType(dataLength, UInteger))
        writer.Flush()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="output">必须能Seek</param>
    ''' <param name="streamWithInfo"></param>
    Public Sub WriteToWaveFile(output As Stream, streamWithInfo As PcmStreamWithInfo)
        If Not output.CanSeek Then
            Throw New Exception
        End If
        Dim numOfRead As Integer
        Dim numOfData As Integer = 0
        Dim StartPosition As Long = output.Position
        WriteWaveHeader(output, streamWithInfo.Format, 0)
        Dim buf(1024 * 1024 - 1) As Byte  '1M
        Do
            numOfRead = streamWithInfo.PcmStream.Read(buf, 0, buf.Length)
            output.Write(buf, 0, numOfRead)
            numOfData += numOfRead
        Loop While numOfRead <> 0
        Dim EndPosition As Long = output.Position
        output.Position = StartPosition
        WriteWaveHeader(output, streamWithInfo.Format, numOfData)
        output.Position = EndPosition
    End Sub
End Module
