using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using SDL2;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public sealed class SdlPcmPlayer : IAudioPlayer, IDisposable
{
    private static readonly object InitLock = new();
    private static int s_instanceCount;
    private bool _disposed;

    public SdlPcmPlayer()
    {
        lock (InitLock)
        {
            if (s_instanceCount == 0)
            {
                var result = SDL.SDL_InitSubSystem(SDL.SDL_INIT_AUDIO);
                if (result < 0)
                {
                    throw new InvalidOperationException($"SDL audio init failed: {SDL.SDL_GetError()}");
                }
            }

            s_instanceCount++;
        }
    }

    public async Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct)
    {
        ThrowIfDisposed();

        if (audio.Format.SampleFormat != PcmSampleFormat.S16LE)
        {
            throw new NotSupportedException($"Unsupported PCM format: {audio.Format.SampleFormat}");
        }

        var desired = new SDL.SDL_AudioSpec
        {
            freq = audio.Format.SampleRate,
            format = SDL.AUDIO_S16LSB,
            channels = (byte)Math.Clamp(audio.Format.Channels, 1, byte.MaxValue),
            samples = 4096,
            callback = null,
            userdata = IntPtr.Zero
        };

        var device = SDL.SDL_OpenAudioDevice(null, 0, ref desired, out var obtained, 0);
        if (device == 0)
        {
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.SDL_GetError()}");
        }

        try
        {
            if (obtained.format != SDL.AUDIO_S16LSB)
            {
                throw new NotSupportedException($"SDL device returned unsupported format: 0x{obtained.format:X}");
            }

            var dataToPlay = ApplyVolume(audio.Data, Math.Clamp(volume, 0, 100));
            var pinned = GCHandle.Alloc(dataToPlay, GCHandleType.Pinned);
            int queueResult;
            try
            {
                queueResult = SDL.SDL_QueueAudio(device, pinned.AddrOfPinnedObject(), (uint)dataToPlay.Length);
            }
            finally
            {
                pinned.Free();
            }
            if (queueResult < 0)
            {
                throw new InvalidOperationException($"SDL queue audio failed: {SDL.SDL_GetError()}");
            }

            SDL.SDL_PauseAudioDevice(device, 0);

            while (SDL.SDL_GetQueuedAudioSize(device) > 0)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(10, ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            SDL.SDL_ClearQueuedAudio(device);
            throw;
        }
        finally
        {
            SDL.SDL_CloseAudioDevice(device);
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
                    SDL.SDL_QuitSubSystem(SDL.SDL_INIT_AUDIO);
                }
            }
        }

        _disposed = true;
    }

    private static byte[] ApplyVolume(byte[] source, int volume)
    {
        if (source.Length == 0)
        {
            return [];
        }

        if (volume >= 100)
        {
            return source.ToArray();
        }

        var scale = volume / 100.0;
        var target = new byte[source.Length];

        for (var i = 0; i + 1 < source.Length; i += 2)
        {
            var sample = (short)(source[i] | (source[i + 1] << 8));
            var scaled = (int)Math.Round(sample * scale);
            scaled = Math.Clamp(scaled, short.MinValue, short.MaxValue);

            target[i] = (byte)(scaled & 0xFF);
            target[i + 1] = (byte)((scaled >> 8) & 0xFF);
        }

        if ((source.Length & 1) == 1)
        {
            target[^1] = source[^1];
        }

        return target;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SdlPcmPlayer));
        }
    }
}
