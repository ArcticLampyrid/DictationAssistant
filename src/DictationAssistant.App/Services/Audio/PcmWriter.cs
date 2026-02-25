using DictationAssistant.Core.Audio;
using Hexa.NET.SDL2;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public class PcmWriter : IDisposable
{
    private static readonly byte[] Byte00_1M = new byte[1048576];
    private static readonly byte[] Byte80_1M = new byte[1048576];

    static PcmWriter()
    {
        Array.Fill(Byte80_1M, (byte)0x80);
    }

    private readonly Stream _outputStream;
    private readonly PcmFormatInfo _formatInfo;
    private readonly bool _leaveOpen;
    private readonly long _headerPosition;
    private long _dataLength;

    public PcmWriter(PcmFormatInfo formatInfo, Stream outputStream, bool leaveOpen = false)
    {
        _formatInfo = formatInfo;
        _outputStream = outputStream;
        _leaveOpen = leaveOpen;

        WriteHeader(_outputStream, _formatInfo, 0);
        _headerPosition = _outputStream.Position;
    }

    public long MillisecondsToSamples(long ms)
    {
        return ((ms / 1000) * _formatInfo.SampleRate) + (((ms % 1000) * _formatInfo.SampleRate) / 1000);
    }

    public long MillisecondsToBytes(long ms)
    {
        return MillisecondsToSamples(ms) * GetBlockAlign();
    }

    private int GetBlockAlign()
    {
        var bytesPerSample = _formatInfo.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        return _formatInfo.Channels * bytesPerSample;
    }

    public long WriteDelay(long ms)
    {
        var len = MillisecondsToBytes(ms);
        var remainingBytes = (int)len;

        var emptyData = _formatInfo.SampleFormat == PcmSampleFormat.U8 ? Byte80_1M : Byte00_1M;

        while (remainingBytes > 0)
        {
            var bytesToWrite = Math.Min(remainingBytes, emptyData.Length);
            _outputStream.Write(emptyData, 0, bytesToWrite);
            remainingBytes -= bytesToWrite;
        }

        _dataLength += len;
        return len;
    }

    public unsafe long Write(PcmAudio input)
    {
        var srcFormat = GetSdlFormat(input.Format.SampleFormat);
        var dstFormat = GetSdlFormat(_formatInfo.SampleFormat);
        var srcChannels = input.Format.Channels;
        var srcSampleRate = input.Format.SampleRate;

        SDLAudioCVT cvt = new SDLAudioCVT();
        SDL.BuildAudioCVT(ref cvt, srcFormat, (byte)srcChannels, srcSampleRate, dstFormat, (byte)_formatInfo.Channels, _formatInfo.SampleRate);

        var bytesPerSample = input.Format.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var srcBlockAlign = srcChannels * bytesPerSample;
        var len = 1024 * srcBlockAlign;
        byte[] buf = new byte[len * cvt.LenMult];
        GCHandle p = GCHandle.Alloc(buf, GCHandleType.Pinned);
        cvt.Buf = (byte*)p.AddrOfPinnedObject();

        input.Data.Position = 0;
        long totalBytes = 0;
        do
        {
            int numOfRead = 0;
            while (numOfRead < len)
            {
                var t = input.Data.Read(buf, numOfRead, len - numOfRead);
                numOfRead += t;
                if (t == 0)
                    break;
            }

            if (numOfRead == 0)
                break;
            cvt.Len = numOfRead;
            SDL.ConvertAudio(ref cvt);
            _outputStream.Write(buf, 0, cvt.LenCvt);
            totalBytes += cvt.LenCvt;
        }
        while (true);
        p.Free();

        var dstBlockAlign = GetBlockAlign();
        if (totalBytes % dstBlockAlign != 0)
        {
            int lenAlign = (int)(dstBlockAlign - (totalBytes % dstBlockAlign));
            byte[] bufAlign = new byte[lenAlign];
            _outputStream.Write(bufAlign, 0, lenAlign);
            totalBytes += lenAlign;
        }

        _dataLength += totalBytes;
        return totalBytes;
    }

    private static ushort GetSdlFormat(PcmSampleFormat format)
    {
        return format switch
        {
            PcmSampleFormat.U8 => 0x0008,
            PcmSampleFormat.S16LE => 0x8010,
            _ => throw new NotSupportedException($"Unsupported format: {format}")
        };
    }

    private static void WriteHeader(Stream output, PcmFormatInfo format, long dataLength)
    {
        var bytesPerSample = format.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var blockAlign = (ushort)(format.Channels * bytesPerSample);
        var byteRate = format.SampleRate * blockAlign;
        var bitsPerSample = (ushort)(bytesPerSample * 8);

        using var writer = new BinaryWriter(output, System.Text.Encoding.UTF8, leaveOpen: true);

        writer.Write("RIFF"u8.ToArray());
        writer.Write(36 + dataLength);
        writer.Write("WAVE"u8.ToArray());

        writer.Write("fmt "u8.ToArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)format.Channels);
        writer.Write(format.SampleRate);
        writer.Write(byteRate);
        writer.Write(blockAlign);
        writer.Write(bitsPerSample);

        writer.Write("data"u8.ToArray());
        writer.Write((uint)dataLength);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_headerPosition > 0)
            {
                _outputStream.Position = 0;
                WriteHeader(_outputStream, _formatInfo, _dataLength);
            }

            if (!_leaveOpen)
            {
                _outputStream.Close();
            }
            else
            {
                _outputStream.Flush();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
