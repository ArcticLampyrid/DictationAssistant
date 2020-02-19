using System;
using System.IO;

namespace DictationAssistant.Audio
{
    public static class PcmAudioHelper
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
