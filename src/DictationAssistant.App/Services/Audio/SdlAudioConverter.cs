using DictationAssistant.Core.Audio;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Services.Audio;

public static class SdlAudioConverter
{
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

    private const ushort AUDIO_U8 = 0x0008;
    private const ushort AUDIO_S16LSB = 0x8010;

    [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    private static extern int SDL_BuildAudioCVT(ref SDL_AudioCVT cvt, ushort src_format, byte src_channels, int src_rate, ushort dst_format, byte dst_channels, int dst_rate);

    [DllImport("SDL2.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    private static extern int SDL_ConvertAudio(ref SDL_AudioCVT cvt);

    private static ushort GetSdlFormat(PcmSampleFormat format)
    {
        return format switch
        {
            PcmSampleFormat.U8 => AUDIO_U8,
            PcmSampleFormat.S16LE => AUDIO_S16LSB,
            _ => throw new NotSupportedException($"Unsupported format: {format}")
        };
    }

    public static long WriteWithResample(Stream output, PcmAudio audio, int targetSampleRate, int targetChannels)
    {
        var srcFormat = GetSdlFormat(audio.Format.SampleFormat);
        var dstFormat = srcFormat;
        var srcChannels = audio.Format.Channels;
        var srcSampleRate = audio.Format.SampleRate;

        SDL_AudioCVT cvt = new SDL_AudioCVT();
        cvt.filters = new IntPtr[10];
        SDL_BuildAudioCVT(ref cvt, srcFormat, (byte)srcChannels, srcSampleRate, dstFormat, (byte)targetChannels, targetSampleRate);

        var bytesPerSample = audio.Format.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var srcBlockAlign = srcChannels * bytesPerSample;
        var len = 1024 * srcBlockAlign;
        byte[] buf = new byte[len * cvt.len_mult];
        GCHandle p = GCHandle.Alloc(buf, GCHandleType.Pinned);
        cvt.buf = p.AddrOfPinnedObject();

        audio.Data.Position = 0;
        long totalBytes = 0;
        do
        {
            int numOfRead = 0;
            while (numOfRead < len)
            {
                var t = audio.Data.Read(buf, numOfRead, len - numOfRead);
                numOfRead += t;
                if (t == 0)
                    break;
            }

            if (numOfRead == 0)
                break;
            cvt.len = numOfRead;
            SDL_ConvertAudio(ref cvt);
            output.Write(buf, 0, cvt.len_cvt);
            totalBytes += cvt.len_cvt;
        }
        while (true);
        p.Free();

        var dstBytesPerSample = audio.Format.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var dstBlockAlign = targetChannels * dstBytesPerSample;
        if (totalBytes % dstBlockAlign != 0)
        {
            int lenAlign = (int)(dstBlockAlign - (totalBytes % dstBlockAlign));
            byte[] bufAlign = new byte[lenAlign];
            output.Write(bufAlign, 0, lenAlign);
            totalBytes += lenAlign;
        }

        return totalBytes;
    }
}
