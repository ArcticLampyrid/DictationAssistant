using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant
{
    public class PcmWriter : IDisposable
    {
        private static byte[] byte00_1M = new byte[1048576];
        private static byte[] byte80_1M = new byte[1048576];
        static PcmWriter()
        {
            SdlAudio.Use();
            for (int i = 0; i < byte80_1M.Length; i++)
            {
                byte80_1M[i] = 0x80;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SDL_AudioCVT
        {
            [MarshalAs(UnmanagedType.Bool)]
            public bool needed;
            public ushort src_format;
            public ushort dst_format;
            public double rate_incr;
            public IntPtr buf;
            public int len;
            public int len_cvt;
            public int len_mult;
            public double len_ratio;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public IntPtr[] filters;
            public int filter_index;
        }

        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_BuildAudioCVT(ref SDL_AudioCVT cvt, ushort src_format, byte src_channels, int src_rate, ushort dst_format, byte dst_channels, int dst_rate);

        [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_ConvertAudio(ref SDL_AudioCVT cvt);

        private Stream OutStream;
        private PcmFormatInfo FormatInfo;
        private readonly bool LeaveOpen;

        public PcmWriter(PcmStreamWithInfo output, bool leaveOpen = false)
        {
            OutStream = output.PcmStream;
            FormatInfo = output.Format;
            LeaveOpen = leaveOpen;
        }
        public long MillisecondsToSamples(long ms)
        {
            return ((ms / 1000) * FormatInfo.Freq) + (((ms % 1000) * FormatInfo.Freq) / 1000);
        }
        public long MillisecondsToBytes(long ms)
        {
            return MillisecondsToSamples(ms) * FormatInfo.BlockAlign;
        }

        internal long BytesToMilliseconds(long byteOffest)
        {
            return SamplesToMilliseconds(byteOffest / FormatInfo.BlockAlign);
        }
        internal long SamplesToMilliseconds(long samples)
        {
            return (samples / FormatInfo.Freq) * 1000 + ((samples % FormatInfo.Freq) * 1000) / FormatInfo.Freq;
        }
        public long WriteDelay(long ms)
        {
            var len = Convert.ToInt32(MillisecondsToBytes(ms));
            var remindByte = len;
            var emptyData = byte00_1M;
            if (PcmSampleFormatHelper.IsUnsigned(FormatInfo.SampleFormat))
                emptyData = byte80_1M;
            while (remindByte > 0) 
            {
                var curByte = Math.Min(remindByte, emptyData.Length);
                OutStream.Write(emptyData, 0, curByte);
                remindByte -= curByte;
            }
            return len;
        }
        public long Write(PcmStreamWithInfo input)
        {
            SDL_AudioCVT cvt = new SDL_AudioCVT();
            cvt.filters = new IntPtr[10];
            SDL_BuildAudioCVT(ref cvt, (ushort)input.Format.SampleFormat, input.Format.Channels, input.Format.Freq, (ushort)FormatInfo.SampleFormat, FormatInfo.Channels, FormatInfo.Freq);
            var len = 1024 * input.Format.BlockAlign;
            byte[] buf = new byte[len * cvt.len_mult];
            GCHandle p = GCHandle.Alloc(buf, GCHandleType.Pinned);
            cvt.buf = p.AddrOfPinnedObject();
            long totalBytes = 0;
            do
            {
                int numOfRead = 0;
                while (numOfRead < len)
                {
                    var t = input.PcmStream.Read(buf, numOfRead, len - numOfRead);
                    numOfRead += t;
                    if (t == 0)
                        break;
                }

                if (numOfRead == 0)
                    break;
                cvt.len = numOfRead;
                SDL_ConvertAudio(ref cvt);
                OutStream.Write(buf, 0, cvt.len_cvt);
                totalBytes += cvt.len_cvt;
            }
            while (true);
            p.Free();
            p = default(GCHandle);
            if ((totalBytes % FormatInfo.BlockAlign != 0))
            {
                int lenAlign = Convert.ToInt32(FormatInfo.BlockAlign - (totalBytes % FormatInfo.BlockAlign));
                byte[] bufAlign = new byte[lenAlign];
                OutStream.Write(bufAlign, 0, lenAlign);
                totalBytes += lenAlign;
            }
            return totalBytes;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (LeaveOpen)
                    OutStream.Flush();
                else
                    OutStream.Close();
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
