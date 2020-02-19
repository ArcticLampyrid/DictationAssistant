namespace DictationAssistant.Audio
{
    public static class PcmSampleFormatHelper
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
}
