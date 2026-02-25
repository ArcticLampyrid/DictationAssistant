namespace DictationAssistant.Core.Audio;

public static class WavWriter
{
    public static void Write(Stream output, PcmAudio audio)
    {
        var dataLength = (int)audio.Data.Length;
        WriteHeader(output, audio.Format.SampleRate, audio.Format.Channels, dataLength);
        audio.Data.Position = 0;
        audio.Data.CopyTo(output);
    }

    public static void WriteHeader(Stream output, int sampleRate, int channels, int dataLength)
    {
        var byteRate = sampleRate * channels * 2;
        var blockAlign = channels * 2;

        using var writer = new BinaryWriter(output, System.Text.Encoding.UTF8, leaveOpen: true);

        writer.Write("RIFF"u8.ToArray());
        writer.Write(36 + dataLength);
        writer.Write("WAVE"u8.ToArray());

        writer.Write("fmt "u8.ToArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)channels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write((short)16);

        writer.Write("data"u8.ToArray());
        writer.Write(dataLength);
    }
}
