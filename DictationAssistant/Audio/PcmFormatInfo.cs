namespace DictationAssistant.Audio
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
}
