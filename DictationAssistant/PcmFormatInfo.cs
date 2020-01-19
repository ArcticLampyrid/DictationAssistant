using System;
using System.IO;

namespace DictationAssistant
{
    public class PcmFormatInfo
    {
        public PcmFormatInfo(int freq, byte channels, PcmSampleFormat sampleFormat)
        {
            this.Freq = freq;
            this.Channels = channels;
            this.SampleFormat = sampleFormat;
        }

        public int Freq { get; }
        public byte Channels { get; }
        public PcmSampleFormat SampleFormat { get; }

        public ushort BlockAlign => (ushort)(Channels * PcmSampleFormatHelper.GetByteSize(SampleFormat));
    }

    public sealed class PcmStreamWithInfo : IDisposable
    {
        public static PcmStreamWithInfo Empty = new PcmStreamWithInfo(Stream.Null, new PcmFormatInfo(44100, 2, PcmSampleFormat.S16));
        public Stream PcmStream { get; }
        public PcmFormatInfo Format { get; }
        public PcmStreamWithInfo(Stream PcmStream, PcmFormatInfo Format)
        {
            this.PcmStream = PcmStream;
            this.Format = Format;
        }
        public void Dispose()
        {
            PcmStream.Dispose();
        }
    }

    public enum PcmSampleFormat : ushort // SDL兼容
    {
        U8 = 0x8,
        S16 = 0x8010
    }

    public class PcmSampleFormatHelper
    {
        public static ushort GetBitSize(PcmSampleFormat a)
        {
            return (ushort)((ushort)a & 0xFF);
        }
        public static ushort GetByteSize(PcmSampleFormat a)
        {
            return (ushort)(((ushort)a & 0xFF) >> 3);
        }
        public static bool IsFloat(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 8)) != 0;
        }
        public static bool IsBigEndian(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 12)) != 0;
        }
        public static bool IsSigned(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 15)) != 0;
        }
        public static bool IsInt(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 8)) == 0;
        }
        public static bool IsLittleEndian(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 12)) == 0;
        }
        public static bool IsUnsigned(PcmSampleFormat a)
        {
            return ((ushort)a & (1 << 15)) == 0;
        }
        public static void SetAsEmptyAudio(PcmSampleFormat a, byte[] data)
        {
            for (var i = 0; i <= data.Length - 1; i++)
            {
                data[i] = 0;
                if (IsUnsigned(a) && i % GetByteSize(a) == 0)
                    data[i] = 0x80;
            }
        }

        public static bool IsEmptyAudio(PcmSampleFormat a, byte[] data, int start, int length)
        {
            for (var i = start; i <= start + length - 1; i++)
            {
                if (IsUnsigned(a) && (i - start) % GetByteSize(a) == 0)
                {
                    if (data[i] != 0x80)
                        return false;
                }
                else if (data[i] != 0)
                    return false;
            }
            return true;
        }
    }

    static class PcmAudioHelper
    {
        public static void WriteWaveHeader(Stream output, PcmFormatInfo format, long dataLength)
        {
            if ((format.SampleFormat != PcmSampleFormat.U8 && format.SampleFormat != PcmSampleFormat.S16))
                throw new NotSupportedException();
            BinaryWriter writer = new BinaryWriter(output);
            ushort wBitsPerSample = PcmSampleFormatHelper.GetBitSize(format.SampleFormat);
            ushort nBlockAlign = format.BlockAlign;
            writer.Write(0x46464952); // Ascii"RIFF"
            writer.Write(Convert.ToUInt32(dataLength + 36));
            writer.Write(0x45564157); // Ascii"WAVE"
            writer.Write(0x20746D66); // Ascii"fmt "
            writer.Write(0x10);
            writer.Write((short)1); // wFormatTag 1=线性PCM编码
            writer.Write(Convert.ToInt16(format.Channels)); // nChannels 通道数
            writer.Write(Convert.ToInt32(format.Freq)); // nSamplesPerSec 采样频率
            writer.Write(Convert.ToInt32(format.Freq * nBlockAlign)); // nAvgBytesPerSec 每秒平均字节数
            writer.Write(Convert.ToUInt16(nBlockAlign)); // nBlockAlign 数据块长度
            writer.Write(Convert.ToUInt16(wBitsPerSample)); // wBitsPerSample PCM位宽
            writer.Write(0x61746164); // Ascii"data"
            writer.Write(Convert.ToUInt32(dataLength));
            writer.Flush();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output">必须能Seek</param>
        /// <param name="streamWithInfo"></param>
        public static void WriteToWaveFile(Stream output, PcmStreamWithInfo streamWithInfo)
        {
            if (!output.CanSeek)
                throw new Exception();
            int numOfRead;
            int numOfData = 0;
            long StartPosition = output.Position;
            WriteWaveHeader(output, streamWithInfo.Format, 0);
            byte[] buf = new byte[1048576];  // 1M
            do
            {
                numOfRead = streamWithInfo.PcmStream.Read(buf, 0, buf.Length);
                output.Write(buf, 0, numOfRead);
                numOfData += numOfRead;
            }
            while (numOfRead != 0);
            long EndPosition = output.Position;
            output.Position = StartPosition;
            WriteWaveHeader(output, streamWithInfo.Format, numOfData);
            output.Position = EndPosition;
        }
    }
}
