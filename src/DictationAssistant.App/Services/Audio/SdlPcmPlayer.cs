using DictationAssistant.Abstractions;
using DictationAssistant.Audio;
using Hexa.NET.SDL2;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public sealed unsafe class SdlPcmPlayer : IAudioPlayer, IDisposable
{
    private const uint SDL_INIT_AUDIO = 0x10;

    private const ushort AUDIO_U8 = 0x0008;
    private const ushort AUDIO_S16LSB = 0x8010;

    private static readonly object InitLock = new();
    private static int s_instanceCount;
    private bool _disposed;

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

    public Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct)
    {
        ThrowIfDisposed();

        var sdlFormat = GetSdlFormat(audio.Format.SampleFormat);
        if (sdlFormat == 0)
        {
            throw new NotSupportedException($"Unsupported PCM format: {audio.Format.SampleFormat}");
        }

        var userdata = new AudioCallbackData
        {
            Stream = audio.ToArray(),
            Position = 0,
            Volume = (byte)(volume * 128 / 100),
            Format = sdlFormat,
            BlockAlign = audio.Format.Channels * GetBytesPerSample(audio.Format.SampleFormat),
            CancellationToken = ct
        };

        return PlayInternalAsync(userdata, audio.Format.SampleRate, audio.Format.Channels);
    }

    private unsafe Task PlayInternalAsync(AudioCallbackData userdata, int sampleRate, int channels)
    {
        var tcs = new TaskCompletionSource();

        var desired = new SDLAudioSpec
        {
            Freq = sampleRate,
            Format = userdata.Format,
            Channels = (byte)Math.Clamp(channels, 1, byte.MaxValue),
            Samples = 1024,
            Callback = default,
            Userdata = default
        };

        var device = OpenDevice(ref desired, out var obtained);
        if (device == 0)
        {
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.GetErrorS()}");
        }

        var gcHandle = GCHandle.Alloc(userdata, GCHandleType.Normal);
        IntPtr userdataPtr = GCHandle.ToIntPtr(gcHandle);

        var callback = new SDL_AudioCallback((udata, streamPtr, len) =>
        {
            if (streamPtr == IntPtr.Zero || len == 0)
            {
                return;
            }

            var handle = GCHandle.FromIntPtr(udata);
            if (!handle.IsAllocated || handle.Target is not AudioCallbackData data)
            {
                var silence = new byte[len];
                Marshal.Copy(silence, 0, streamPtr, len);
                return;
            }

            if (data.CancellationToken.IsCancellationRequested || data.Position >= data.Stream.Length)
            {
                var silence = new byte[len];
                if (data.Format == AUDIO_U8)
                {
                    Array.Fill(silence, (byte)128);
                }
                Marshal.Copy(silence, 0, streamPtr, len);
                return;
            }

            var bytesToRead = Math.Min(len, (int)(data.Stream.Length - data.Position));
            var buffer = new byte[bytesToRead];
            Array.Copy(data.Stream, data.Position, buffer, 0, bytesToRead);

            if (data.Volume < 128 && bytesToRead > 0)
            {
                var tempBuffer = new byte[bytesToRead];
                Array.Copy(buffer, tempBuffer, bytesToRead);
                fixed (byte* srcPtr = tempBuffer)
                fixed (byte* dstPtr = buffer)
                {
                    SDL.MixAudioFormat(dstPtr, srcPtr, data.Format, (uint)bytesToRead, data.Volume);
                }
            }

            Marshal.Copy(buffer, 0, streamPtr, bytesToRead);

            if (bytesToRead < len)
            {
                var remaining = len - bytesToRead;
                var silence = new byte[remaining];
                if (data.Format == AUDIO_U8)
                {
                    Array.Fill(silence, (byte)128);
                }
                Marshal.Copy(silence, 0, streamPtr + bytesToRead, remaining);
            }

            data.Position += bytesToRead;

            if (data.Position >= data.Stream.Length)
            {
                data.IsCompleted = true;
            }
        });

        IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        void* userdataVoidPtr = (void*)userdataPtr.ToPointer();
        desired.Callback = (void*)callbackPtr;
        desired.Userdata = userdataVoidPtr;

        SDL.CloseAudioDevice(device);
        device = OpenDevice(ref desired, out obtained);
        if (device == 0)
        {
            gcHandle.Free();
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.GetErrorS()}");
        }

        if (obtained.Format != userdata.Format)
        {
            gcHandle.Free();
            SDL.CloseAudioDevice(device);
            throw new NotSupportedException($"SDL device returned unsupported format: 0x{obtained.Format:X}");
        }

        var thread = new Thread(() =>
        {
            try
            {
                SDL.PauseAudioDevice(device, 0);

                while (!userdata.IsCompleted && !userdata.CancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(10);
                }

                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            finally
            {
                gcHandle.Free();
                SDL.CloseAudioDevice(device);
                tcs.TrySetResult();
            }
        })
        {
            IsBackground = true
        };
        thread.Start();

        return tcs.Task;
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

    private static unsafe uint OpenDevice(ref SDLAudioSpec desired, out SDLAudioSpec obtained)
    {
        fixed (SDLAudioSpec* desiredPtr = &desired)
        {
            SDLAudioSpec obtainedSpec;
            var device = SDL.OpenAudioDevice((byte*)null, 0, desiredPtr, &obtainedSpec, 0);
            obtained = obtainedSpec;
            return device;
        }
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

    private static int GetBytesPerSample(PcmSampleFormat format)
    {
        return format switch
        {
            PcmSampleFormat.U8 => 1,
            PcmSampleFormat.S16LE => 2,
            _ => 2
        };
    }

    private class AudioCallbackData
    {
        public byte[] Stream { get; set; } = [];
        public long Position { get; set; }
        public byte Volume { get; set; }
        public ushort Format { get; set; }
        public int BlockAlign { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public bool IsCompleted { get; set; }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SdlPcmPlayer));
        }
    }
}
