using DictationAssistant.App.Audio;
using Hexa.NET.SDL2;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant.App.Audio;

public static class SdlAudioConverter
{
    private const ushort AUDIO_U8 = 0x0008;
    private const ushort AUDIO_S16LSB = 0x8010;

    private static ushort GetSdlFormat(PcmSampleFormat format)
    {
        return format switch
        {
            PcmSampleFormat.U8 => AUDIO_U8,
            PcmSampleFormat.S16LE => AUDIO_S16LSB,
            _ => throw new NotSupportedException($"Unsupported format: {format}")
        };
    }

    public static unsafe long WriteWithResample(Stream output, PcmAudio audio, int targetSampleRate, int targetChannels)
    {
        var srcFormat = GetSdlFormat(audio.Format.SampleFormat);
        var dstFormat = srcFormat;
        var srcChannels = audio.Format.Channels;
        var srcSampleRate = audio.Format.SampleRate;

        SDLAudioCVT cvt = new SDLAudioCVT();
        SDL.BuildAudioCVT(ref cvt, srcFormat, (byte)srcChannels, srcSampleRate, dstFormat, (byte)targetChannels, targetSampleRate);

        var bytesPerSample = audio.Format.SampleFormat == PcmSampleFormat.U8 ? 1 : 2;
        var srcBlockAlign = srcChannels * bytesPerSample;
        var len = 1024 * srcBlockAlign;
        byte[] buf = new byte[len * cvt.LenMult];
        GCHandle p = GCHandle.Alloc(buf, GCHandleType.Pinned);
        cvt.Buf = (byte*)p.AddrOfPinnedObject();

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
            cvt.Len = numOfRead;
            SDL.ConvertAudio(ref cvt);
            output.Write(buf, 0, cvt.LenCvt);
            totalBytes += cvt.LenCvt;
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
