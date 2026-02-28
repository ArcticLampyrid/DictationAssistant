using DictationAssistant.App.Abstractions;
using DictationAssistant.App.Audio;
using Hexa.NET.SDL2;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Audio;

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
            PcmStream = audio.Data,
            SdlVolume = (byte)(volume * 128 / 100),
            SdlFormat = sdlFormat,
            BlockAlign = audio.Format.Channels * GetBytesPerSample(audio.Format.SampleFormat)
        };

        var callback = new SDL_AudioCallback((udata, streamPtr, len) =>
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


            int numOfRead = 0;
            if (!data.IsCompleted)
            {
                var buffer = new byte[len];
                while (numOfRead < len)
                {
                    var t = data.PcmStream.Read(buffer, numOfRead, len - numOfRead);
                    numOfRead += t;
                    if (t == 0)
                        break;
                }
                if (data.SdlVolume < 128)
                {
                    fixed (byte* srcPtr = buffer)
                    {
                        SDL.MixAudioFormat((byte*)streamPtr, srcPtr, data.SdlFormat, (uint)numOfRead, data.SdlVolume);
                    }
                }
                else
                {
                    Marshal.Copy(buffer, 0, streamPtr, numOfRead);
                }
            }

            if (numOfRead < len)
            {
                var remaining = len - numOfRead;
                var silence = new byte[remaining];
                if (SdlFormatIsUnsigned(data.SdlFormat))
                {
                    Array.Fill(silence, (byte)128);
                }
                Marshal.Copy(silence, 0, streamPtr + numOfRead, remaining);
                data.IsCompleted = true;
            }
        });

        IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        var userdataGcHandle = GCHandle.Alloc(userdata, GCHandleType.Normal);

        var tcs = new TaskCompletionSource();
        var desired = new SDLAudioSpec
        {
            Freq = audio.Format.SampleRate,
            Format = sdlFormat,
            Channels = (byte)Math.Clamp(audio.Format.Channels, 1, byte.MaxValue),
            Samples = 1024,
            Callback = (void*)callbackPtr,
            Userdata = GCHandle.ToIntPtr(userdataGcHandle).ToPointer()
        };

        var device = OpenDevice(ref desired, out var obtained);
        if (device == 0)
        {
            userdataGcHandle.Free();
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.GetErrorS()}");
        }

        if (obtained.Format != userdata.SdlFormat)
        {
            userdataGcHandle.Free();
            SDL.CloseAudioDevice(device);
            throw new NotSupportedException($"SDL device returned unsupported format: 0x{obtained.Format:X}");
        }
        SDL.PauseAudioDevice(device, 0);

        var thread = new Thread(() =>
        {
            try
            {
                while (!userdata.IsCompleted && !ct.IsCancellationRequested)
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            finally
            {
                SDL.CloseAudioDevice(device);
                userdataGcHandle.Free();
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

    private static uint OpenDevice(ref SDLAudioSpec desired, out SDLAudioSpec obtained)
    {
        fixed (SDLAudioSpec* obtainedPtr = &obtained)
        fixed (SDLAudioSpec* desiredPtr = &desired)
        {
            var device = SDL.OpenAudioDevice((byte*)null, 0, desiredPtr, obtainedPtr, 0);
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

    private static bool SdlFormatIsUnsigned(ushort sdlFormat)
    {
        return (sdlFormat & (1 << 15)) == 0;
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
        public required Stream PcmStream { get; set; }
        public byte SdlVolume { get; set; }
        public ushort SdlFormat { get; set; }
        public int BlockAlign { get; set; }
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
