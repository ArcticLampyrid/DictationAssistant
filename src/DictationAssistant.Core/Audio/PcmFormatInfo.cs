namespace DictationAssistant.Core.Audio;

public readonly record struct PcmFormatInfo(int SampleRate, int Channels, PcmSampleFormat SampleFormat);
