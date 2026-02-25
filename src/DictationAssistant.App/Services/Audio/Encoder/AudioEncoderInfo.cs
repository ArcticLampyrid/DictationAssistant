using DictationAssistant.Core.Audio;

namespace DictationAssistant.App.Services.Audio.Encoder;

public abstract class AudioEncoderInfo
{
    protected AudioEncoderInfo(string name, string extension)
    {
        Name = name;
        Extension = extension;
    }

    public string Name { get; }
    public string Extension { get; }

    public abstract PcmAudio CreateEncoder(PcmFormatInfo format, string path, object? encodeSettings);

    public PcmAudio CreateEncoder(PcmFormatInfo format, string path)
    {
        return CreateEncoder(format, path, null);
    }

    public override string ToString() => Name;
}
