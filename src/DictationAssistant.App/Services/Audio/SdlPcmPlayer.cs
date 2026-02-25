using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using Hexa.NET.SDL2;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public sealed class SdlPcmPlayer : IAudioPlayer, IDisposable
{
    private const uint SDL_INIT_AUDIO = 0x10;
    private const ushort AUDIO_S16LSB = 0x8010;

    private static readonly object InitLock = new();
    private static int s_instanceCount;
    private bool _disposed;

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

    public async Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct)
    {
        ThrowIfDisposed();

        if (audio.Format.SampleFormat != PcmSampleFormat.S16LE)
        {
            throw new NotSupportedException($"Unsupported PCM format: {audio.Format.SampleFormat}");
        }

        var desired = new SDLAudioSpec
        {
            Freq = audio.Format.SampleRate,
            Format = AUDIO_S16LSB,
            Channels = (byte)Math.Clamp(audio.Format.Channels, 1, byte.MaxValue),
            Samples = 4096,
            Callback = default,
            Userdata = default
        };

        var device = OpenDevice(ref desired, out var obtained);
        if (device == 0)
        {
            throw new InvalidOperationException($"SDL open audio device failed: {SDL.GetErrorS()}");
        }

        try
        {
            if (obtained.Format != AUDIO_S16LSB)
            {
                throw new NotSupportedException($"SDL device returned unsupported format: 0x{obtained.Format:X}");
            }

            var dataToPlay = ApplyVolume(audio.Data, Math.Clamp(volume, 0, 100));
            var queueResult = QueuePcm(device, dataToPlay);
            if (queueResult < 0)
            {
                throw new InvalidOperationException($"SDL queue audio failed: {SDL.GetErrorS()}");
            }

            SDL.PauseAudioDevice(device, 0);

            while (SDL.GetQueuedAudioSize(device) > 0)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(10, ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            SDL.ClearQueuedAudio(device);
            throw;
        }
        finally
        {
            SDL.CloseAudioDevice(device);
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

    private static unsafe int QueuePcm(uint device, byte[] data)
    {
        fixed (byte* ptr = data)
        {
            return SDL.QueueAudio(device, ptr, (uint)data.Length);
        }
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
