using DictationAssistant.Audio;
using ManagedBass;
using System.IO;

namespace DictationAssistant.App.Services.Audio;

public sealed class BassDecodeStream : Stream
{
    private int _channel;
    private int _position;
    private int _bufferStart;
    private int _bufferLength;
    private readonly byte[] _buffer = new byte[512];
    private bool _disposed;

    public PcmFormatInfo Format { get; }

    public BassDecodeStream(int channel)
    {
        _channel = channel;
        var channelInfo = Bass.ChannelGetInfo(channel);
        Format = new PcmFormatInfo(
            channelInfo.Frequency,
            channelInfo.Channels,
            PcmSampleFormat.S16LE
        );
    }

    public static BassDecodeStream? CreateFromFile(string filePath)
    {
        var stream = Bass.CreateStream(filePath, Flags: BassFlags.Decode | BassFlags.Unicode);
        if (stream == 0)
        {
            return null;
        }
        return new BassDecodeStream(stream);
    }

    public static BassDecodeStream? CreateFromStream(Stream inputStream)
    {
        if (!BassAudioDecoder.EnsureInitialized())
        {
            return null;
        }

        var fileProcs = new FileProcedures
        {
            Close = _ => { },
            Length = _ => inputStream.Length,
            Read = (buffer, length, _) =>
            {
                var bytes = new byte[length];
                var bytesRead = inputStream.Read(bytes, 0, length);
                if (bytesRead > 0)
                {
                    System.Runtime.InteropServices.Marshal.Copy(bytes, 0, buffer, bytesRead);
                }
                return bytesRead;
            },
            Seek = (offset, _) =>
            {
                try
                {
                    inputStream.Seek(offset, SeekOrigin.Begin);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        };

        var stream = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Decode | BassFlags.Unicode, fileProcs);
        if (stream == 0)
        {
            return null;
        }

        return new BassDecodeStream(stream);
    }

    public override bool CanRead => true;
    public override bool CanWrite => false;
    public override bool CanSeek => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    public override void SetLength(long value) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void Flush() { }

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_channel == 0 || _disposed)
            return 0;

        int outputLength = 0;
        int numOfNeed = count;

        while (numOfNeed > 0)
        {
            int bufferUnread = _bufferLength - _bufferStart;
            if (bufferUnread > 0)
            {
                if (bufferUnread > numOfNeed)
                {
                    Array.Copy(_buffer, _bufferStart, buffer, offset + outputLength, numOfNeed);
                    _position += numOfNeed;
                    _bufferStart += numOfNeed;
                    outputLength += numOfNeed;
                    numOfNeed = 0;
                }
                else
                {
                    Array.Copy(_buffer, _bufferStart, buffer, offset + outputLength, bufferUnread);
                    _position += bufferUnread;
                    _bufferStart = 0;
                    _bufferLength = 0;
                    outputLength += bufferUnread;
                    numOfNeed -= bufferUnread;
                }
            }
            else if (numOfNeed > _buffer.Length)
            {
                var handle = System.Runtime.InteropServices.GCHandle.Alloc(buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var numOfOutput = Bass.ChannelGetData(
                        _channel,
                        (IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset + outputLength),
                        numOfNeed
                    );

                    if (numOfOutput > 0)
                    {
                        _position += numOfOutput;
                        outputLength += numOfOutput;
                        numOfNeed -= numOfOutput;
                    }
                    else
                    {
                        break;
                    }
                }
                finally
                {
                    handle.Free();
                }
            }
            else
            {
                _bufferStart = 0;
                _bufferLength = Bass.ChannelGetData(_channel, _buffer, _buffer.Length);
                if (_bufferLength < 0)
                {
                    _bufferLength = 0;
                    break;
                }
            }
        }

        return outputLength;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _channel != 0)
            {
                Bass.StreamFree(_channel);
                _channel = 0;
            }
            _disposed = true;
        }
        base.Dispose(disposing);
    }

    public override void Close()
    {
        Dispose(true);
        base.Close();
    }
}
