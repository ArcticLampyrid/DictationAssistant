using DictationAssistant.Core.Audio;

namespace DictationAssistant.App.Services.Audio.Encoder;

public class ExternalAudioEncoderInfo : AudioEncoderInfo
{
    public string EncoderFileName { get; }
    public string EncoderArgumentsFormat { get; }

    public ExternalAudioEncoderInfo(string name, string extension, string encoderFileName, string encoderArgumentsFormat)
        : base(name, extension)
    {
        EncoderFileName = encoderFileName;
        EncoderArgumentsFormat = encoderArgumentsFormat;
    }

    public override PcmAudio CreateEncoder(PcmFormatInfo format, string path, object? encodeSettings)
    {
        return new PcmAudio
        {
            Data = new ExternalAudioEncoder(format, path, EncoderFileName, EncoderArgumentsFormat),
            Format = format
        };
    }
}
