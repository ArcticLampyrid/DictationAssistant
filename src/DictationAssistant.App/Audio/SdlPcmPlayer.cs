using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Audio;
using Hexa.NET.SDL2;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Audio;

public sealed class SdlPcmPlayer : IAudioPlayer, IDisposable
{
    private const uint SDL_INIT_AUDIO = 0x10;

    private const ushort AUDIO_U8 = 0x0008;
    private const ushort AUDIO_S16LSB = 0x8010;

    private static readonly object InitLock = new();
    private static int s_instanceCount;
    private bool _disposed;

    private int _sdlVolume = 128; // Default volume is 100%

    public int Volume
    {
        get => (int)Math.Round(_sdlVolume / 128.0 * 100);
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Volume must be between 0 and 100.");
            }
            _sdlVolume = (int)Math.Round(value / 100.0 * 128);
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SDL_AudioCallback(IntPtr userdata, IntPtr stream, int len);

    public SdlPcmPlayer()
    {
        lock (InitLock)
        {
            if (s_instanceCount == 0)
            {
                var result = SDL.InitSubSystem(SDL_INIT_AUDIO);
                if (result < 0)
                {
                    throw new InvalidOperationException($"SDL audio init failed: {SDL.GetErrorS()}");
                }
            }

            s_instanceCount++;
        }
    }

    public async Task PlayAsync(PcmAudio audio, CancellationToken ct)
    {
        ThrowIfDisposed();

        var sdlFormat = GetSdlFormat(audio.Format.SampleFormat);
        if (sdlFormat == 0)
        {
            throw new NotSupportedException($"Unsupported PCM format: {audio.Format.SampleFormat}");
        }

        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var userdata = new AudioCallbackData
        {
            PcmStream = audio.Data,
            SdlFormat = sdlFormat,
            Tcs = tcs
        };

        var callback = new SDL_AudioCallback(AudioCallback);

        IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        var userdataGcHandle = GCHandle.Alloc(userdata, GCHandleType.Normal);

        var device = OpenDevice(audio.Format, sdlFormat, callbackPtr, userdataGcHandle, out var obtained);
        if (device == 0)
        {
            userdataGcHandle.Free();
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.GetErrorS()}");
        }

        if (obtained.Format != sdlFormat)
        {
            userdataGcHandle.Free();
            SDL.CloseAudioDevice(device);
            throw new NotSupportedException($"SDL device returned unsupported format: 0x{obtained.Format:X}");
        }

        SDL.PauseAudioDevice(device, 0);

        // Pin the callback delegate to prevent GC collection while SDL is using it
        var callbackGcHandle = GCHandle.Alloc(callback);

        using var ctr = ct.Register(() => tcs.TrySetCanceled(ct));
        try
        {
            await tcs.Task.ConfigureAwait(false);
        }
        finally
        {
            SDL.CloseAudioDevice(device);
            userdataGcHandle.Free();
            callbackGcHandle.Free();
        }
    }

    private unsafe void AudioCallback(IntPtr udata, IntPtr streamPtr, int len)
    {
        if (streamPtr == IntPtr.Zero || len == 0)
        {
            return;
        }

        var handle = GCHandle.FromIntPtr(udata);
        if (!handle.IsAllocated || handle.Target is not AudioCallbackData data)
        {
            throw new InvalidOperationException("Invalid SDL audio callback userdata.");
        }

        Span<byte> streamSpan = new(streamPtr.ToPointer(), len);
        int numOfRead = 0;
        if (!data.IsCompleted)
        {
            if (_sdlVolume < 128)
            {
                // Read PCM data into a temporary buffer
                var buffer = new byte[len];
                while (numOfRead < len)
                {
                    var t = data.PcmStream.Read(buffer, numOfRead, len - numOfRead);
                    numOfRead += t;
                    if (t == 0)
                        break;
                }
                // Fill the stream buffer with silence before mixing to avoid noise
                streamSpan[..numOfRead].Fill(SdlFormatIsUnsigned(data.SdlFormat) ? (byte)128 : (byte)0);
                // Mix the PCM data with volume control into the stream buffer
                fixed (byte* srcPtr = buffer)
                {
                    SDL.MixAudioFormat((byte*)streamPtr, srcPtr, data.SdlFormat, (uint)numOfRead, (byte)_sdlVolume);
                }
            }
            else
            {
                // Read PCM data directly into the stream buffer without volume control
                while (numOfRead < len)
                {
                    var t = data.PcmStream.Read(streamSpan[numOfRead..]);
                    numOfRead += t;
                    if (t == 0)
                        break;
                }
            }
        }

        if (numOfRead < len)
        {
            // Fill the remaining stream buffer with silence to avoid noise
            streamSpan[numOfRead..].Fill(SdlFormatIsUnsigned(data.SdlFormat) ? (byte)128 : (byte)0);
            data.IsCompleted = true;
            data.Tcs.TrySetResult();
        }
    }

    private static unsafe uint OpenDevice(PcmFormatInfo format, ushort sdlFormat, IntPtr callbackPtr, GCHandle userdataGcHandle, out SDLAudioSpec obtained)
    {
        var desired = new SDLAudioSpec
        {
            Freq = format.SampleRate,
            Format = sdlFormat,
            Channels = (byte)Math.Clamp(format.Channels, 1, byte.MaxValue),
            Samples = 1024,
            Callback = (void*)callbackPtr,
            Userdata = GCHandle.ToIntPtr(userdataGcHandle).ToPointer()
        };

        fixed (SDLAudioSpec* obtainedPtr = &obtained)
        {
            var device = SDL.OpenAudioDevice((byte*)null, 0, &desired, obtainedPtr, 0);
            return device;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (InitLock)
        {
            if (s_instanceCount > 0)
            {
                s_instanceCount--;
                if (s_instanceCount == 0)
                {
                    SDL.QuitSubSystem(SDL_INIT_AUDIO);
                }
            }
        }

        _disposed = true;
    }

    private static ushort GetSdlFormat(PcmSampleFormat format)
    {
        return format switch
        {
            PcmSampleFormat.U8 => AUDIO_U8,
            PcmSampleFormat.S16LE => AUDIO_S16LSB,
            _ => 0
        };
    }

    private static bool SdlFormatIsUnsigned(ushort sdlFormat)
    {
        return (sdlFormat & (1 << 15)) == 0;
    }

    private class AudioCallbackData
    {
        public required Stream PcmStream { get; set; }
        public ushort SdlFormat { get; set; }
        public bool IsCompleted { get; set; }
        public required TaskCompletionSource Tcs { get; set; }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SdlPcmPlayer));
        }
    }
}
