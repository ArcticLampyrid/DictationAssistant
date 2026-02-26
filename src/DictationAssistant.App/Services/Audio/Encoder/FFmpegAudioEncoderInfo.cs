using System;
using System.Threading;
using System.Threading.Tasks;
using DictationAssistant.App.Audio;

namespace DictationAssistant.App.Services.Audio.Encoder;

public class FFmpegAudioEncoderStream : Stream
{
    private readonly DictationAssistant.App.Audio.FFmpegAudioEncoder _encoder;
    private bool _disposed;

    public FFmpegAudioEncoderStream(DictationAssistant.App.Audio.FFmpegAudioEncoder encoder)
    {
        _encoder = encoder;
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

    public override void Flush() => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => _encoder.InputStream.Write(buffer, offset, count);

    public Task FinishAsync(CancellationToken cancellationToken = default) => _encoder.FinishAsync(cancellationToken);

    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;

        if (disposing)
        {
            _encoder.DisposeAsync().AsTask().Wait();
        }
    }
}

public class FFmpegAudioEncoderInfo : AudioEncoderInfo
{
    public string Format { get; }

    public FFmpegAudioEncoderInfo(string format) : base(GetFormatName(format), GetFormatExtension(format))
    {
        Format = format;
    }

    public override PcmAudio CreateEncoder(PcmFormatInfo format, string path, object? encodeSettings)
    {
        var encoder = new DictationAssistant.App.Audio.FFmpegAudioEncoder(path, format.SampleRate, format.Channels, Format);
        return new PcmAudio
        {
            Data = new FFmpegAudioEncoderStream(encoder),
            Format = format
        };
    }

    private static string GetFormatName(string format)
    {
        return format.ToLowerInvariant() switch
        {
            "mp3" => "MP3 (via FFmpeg)",
            "opus" => "Opus (via FFmpeg)",
            _ => $"{format.ToUpperInvariant()} (via FFmpeg)"
        };
    }

    private static string GetFormatExtension(string format)
    {
        return format.ToLowerInvariant() switch
        {
            "mp3" => "mp3",
            "opus" => "opus",
            _ => format.ToLowerInvariant()
        };
    }
}
