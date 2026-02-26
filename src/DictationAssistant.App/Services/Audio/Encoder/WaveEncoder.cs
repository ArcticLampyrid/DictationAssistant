using System;
using System.IO;
using DictationAssistant.App.Audio;

namespace DictationAssistant.App.Services.Audio.Encoder;

public class WaveEncoder : Stream
{
    private sealed class WaveEncoderInfoImpl : AudioEncoderInfo
    {
        public WaveEncoderInfoImpl() : base("Waveform Audio", "wav")
        {
        }

        public override PcmAudio CreateEncoder(PcmFormatInfo format, string path, object? encodeSettings)
        {
            return new PcmAudio
            {
                Data = new WaveEncoder(format, path),
                Format = format
            };
        }
    }

    public static AudioEncoderInfo EncoderInfo { get; } = new WaveEncoderInfoImpl();

    private readonly Stream _baseStream;
    private readonly long _headerSize;
    private readonly PcmFormatInfo _pcmFormatInfo;

    public WaveEncoder(PcmFormatInfo pcmFormatInfo, string path)
    {
        _pcmFormatInfo = pcmFormatInfo;
        _baseStream = File.Open(path, FileMode.Create);
        WriteWaveHeader(_baseStream, _pcmFormatInfo, 0);
        _headerSize = _baseStream.Length;
    }

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush() => _baseStream.Flush();

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => _baseStream.Write(buffer, offset, count);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _baseStream.Position = 0;
            WriteWaveHeader(_baseStream, _pcmFormatInfo, _baseStream.Length - _headerSize);
            _baseStream.Dispose();
        }
    }

    private static void WriteWaveHeader(Stream output, PcmFormatInfo format, long dataLength)
    {
        if (format.SampleFormat != PcmSampleFormat.U8 && format.SampleFormat != PcmSampleFormat.S16LE)
            throw new NotSupportedException("Only U8 and S16LE formats are supported");

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
}
