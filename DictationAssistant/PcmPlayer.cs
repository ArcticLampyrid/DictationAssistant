using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace DictationAssistant
{
    public class PcmPlayer : IDisposable
    {
        public enum AudioPlayState
        {
            Stopped = 0,
            Playing = 1,
            Paused = 2
        }
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint SDL_OpenAudioDevice(IntPtr device, [MarshalAs(UnmanagedType.Bool)] bool iscapture, ref SDL_AudioSpec desired, ref SDL_AudioSpec obtained, int allowed_changes);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_PauseAudioDevice(uint dev, [MarshalAs(UnmanagedType.Bool)] bool pause_on);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_CloseAudioDevice(uint dev);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_LockAudioDevice(uint dev);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_UnlockAudioDevice(uint dev);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_GetAudioDeviceStatus(uint dev);
        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_MixAudioFormat(IntPtr dst, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] src, ushort format, int len, int volume);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_AudioCallback(IntPtr userdata, IntPtr stream, int len);

        [StructLayout(LayoutKind.Sequential)]
        private struct SDL_AudioSpec
        {
            public int freq;
            public ushort format;
            public byte channels;
            public byte silence;
            public ushort samples;
            public ushort padding;
            public uint size;
            public SDL_AudioCallback callback;
            public IntPtr userdata;
        }

        static PcmPlayer()
        {
            SdlAudio.Use();
        }

        public AudioPlayState State
        {
            get
            {
                if (AudioDeviceID == 0)
                    return AudioPlayState.Stopped;
                switch (SDL_GetAudioDeviceStatus(AudioDeviceID))
                {
                    case 0:
                        {
                            return AudioPlayState.Stopped;
                        }

                    case 1:
                        {
                            return AudioPlayState.Playing;
                        }

                    case 2:
                        {
                            return AudioPlayState.Paused;
                        }

                    default:
                        {
                            return AudioPlayState.Stopped;
                        }
                }
            }
        }

        private Stream PcmStream;
        private uint AudioDeviceID;
        private GCHandle gc_handle;
        private byte SdlVolume;
        private List<EventHandler> PlayCompletedEventHandlerList = new List<EventHandler>();
        public event EventHandler PlayCompleted;
        private SDL_AudioSpec audioSpec;
        private PcmSampleFormat Format;
        private void Callback_fill(IntPtr userdata, IntPtr stream, int len)
        {
            if (len == 0 || stream == IntPtr.Zero)
                return;

            byte[] data = new byte[len];
            PcmSampleFormatHelper.SetAsEmptyAudio(Format, data);
            Marshal.Copy(data, 0, stream, len);
            if (PcmStream != null)
            {
                int numOfRead = 0;
                while (numOfRead < len)
                {
                    var t = PcmStream.Read(data, numOfRead, len - numOfRead);
                    numOfRead += t;
                    if (t == 0)
                        break;
                }

                if (numOfRead == 0)
                {
                    PcmStream.Dispose();
                    PcmStream = null;

                    Thread t = new Thread(() =>
                    {
                        Dispose();
                        PlayCompleted?.Invoke(this, EventArgs.Empty);
                    });
                    t.Start();
                }
                else
                    SDL_MixAudioFormat(stream, data, (ushort)Format, numOfRead, SdlVolume);
            }
        }

        public PcmPlayer(PcmStreamWithInfo pcm, byte volume = 100)
        {
            PcmStream = pcm.PcmStream;
            SdlVolume = System.Convert.ToByte(volume * 128 / 100);
            Format = pcm.Format.SampleFormat;
            audioSpec = new SDL_AudioSpec()
            {
                freq = pcm.Format.Freq,
                format = (ushort)Format,
                channels = pcm.Format.Channels,
                samples = 1024,
                callback = Callback_fill,
                userdata = IntPtr.Zero
            };
            var obtained = default(SDL_AudioSpec);
            AudioDeviceID = SDL_OpenAudioDevice(IntPtr.Zero, false, ref audioSpec, ref obtained, 0);
        }
        public void Play()
        {
            if (!gc_handle.IsAllocated)
                gc_handle = GCHandle.Alloc(this, GCHandleType.Normal);
            SDL_PauseAudioDevice(AudioDeviceID, false);
        }
        public void Pause()
        {
            SDL_LockAudioDevice(AudioDeviceID);
            if (AudioDeviceID != 0)
            {
                SDL_PauseAudioDevice(AudioDeviceID, true);
                SDL_UnlockAudioDevice(AudioDeviceID);
            }
            if (gc_handle.IsAllocated)
                gc_handle.Free();
        }

        private bool disposedValue; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (AudioDeviceID != 0)
                {
                    SDL_LockAudioDevice(AudioDeviceID);
                    if (AudioDeviceID != 0)
                    {
                        SDL_CloseAudioDevice(AudioDeviceID);
                        SDL_UnlockAudioDevice(AudioDeviceID);
                        AudioDeviceID = 0;
                    }
                }

                if (disposing)
                {
                    if (PcmStream != null)
                        PcmStream.Dispose();
                    if (gc_handle.IsAllocated)
                        gc_handle.Free();
                }

                PcmStream = null;
            }
            disposedValue = true;
        }


        ~PcmPlayer()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(false);
        }

        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
