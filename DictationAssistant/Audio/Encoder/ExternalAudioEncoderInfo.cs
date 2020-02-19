namespace DictationAssistant.Audio.Encoder
{
    public class ExternalAudioEncoderInfo : AudioEncoderInfo
    {
        public string EncoderFileName { get; }
        public string EncoderArgumentsFormat { get; }

        public ExternalAudioEncoderInfo(string name, string extension, string encoderFileName, string encoderArgumentsFormat) : base(name, extension)
        {
            this.EncoderFileName = encoderFileName;
            this.EncoderArgumentsFormat = encoderArgumentsFormat;
        }

        public override PcmStreamWithInfo CreateEncoder(PcmFormatInfo format, string path, object encodeSettings)
        {
            return new PcmStreamWithInfo(new ExternalAudioEncoder(format, path, EncoderFileName, EncoderArgumentsFormat), format);
        }
    }
}
