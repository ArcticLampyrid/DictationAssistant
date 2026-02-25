using System.Buffers.Binary;

namespace DictationAssistant.Core.Audio;

public static class WavReader
{
    public static PcmAudio ReadPcmAudio(byte[] wavBytes)
    {
        if (!TryReadPcmAudio(wavBytes, out var audio, out var error))
        {
            throw new InvalidDataException(error ?? "Invalid wav data.");
        }

        return audio!;
    }

    public static bool TryReadPcmAudio(byte[] wavBytes, out PcmAudio? audio, out string? error)
    {
        audio = null;
        error = null;

        if (wavBytes.Length < 44)
        {
            error = "WAV payload is too small.";
            return false;
        }

        if (!wavBytes.AsSpan(0, 4).SequenceEqual("RIFF"u8) || !wavBytes.AsSpan(8, 4).SequenceEqual("WAVE"u8))
        {
            error = "WAV header is missing RIFF/WAVE signature.";
            return false;
        }

        var offset = 12;
        var sampleRate = 0;
        var channels = 0;
        var bitsPerSample = 0;
        var formatTag = 0;
        ReadOnlySpan<byte> pcmPayload = default;

        while (offset + 8 <= wavBytes.Length)
        {
            var chunkId = wavBytes.AsSpan(offset, 4);
            var chunkSize = BinaryPrimitives.ReadInt32LittleEndian(wavBytes.AsSpan(offset + 4, 4));
            if (chunkSize < 0)
            {
                error = "WAV chunk size is invalid.";
                return false;
            }

            var chunkDataOffset = offset + 8;
            if (chunkDataOffset + chunkSize > wavBytes.Length)
            {
                error = "WAV chunk exceeds payload bounds.";
                return false;
            }

            if (chunkId.SequenceEqual("fmt "u8))
            {
                if (chunkSize < 16)
                {
                    error = "WAV fmt chunk is too small.";
                    return false;
                }

                formatTag = BinaryPrimitives.ReadUInt16LittleEndian(wavBytes.AsSpan(chunkDataOffset, 2));
                channels = BinaryPrimitives.ReadUInt16LittleEndian(wavBytes.AsSpan(chunkDataOffset + 2, 2));
                sampleRate = BinaryPrimitives.ReadInt32LittleEndian(wavBytes.AsSpan(chunkDataOffset + 4, 4));
                bitsPerSample = BinaryPrimitives.ReadUInt16LittleEndian(wavBytes.AsSpan(chunkDataOffset + 14, 2));
            }
            else if (chunkId.SequenceEqual("data"u8))
            {
                pcmPayload = wavBytes.AsSpan(chunkDataOffset, chunkSize);
            }

            var paddedChunkSize = (chunkSize + 1) & ~1;
            offset = chunkDataOffset + paddedChunkSize;
        }

        if (formatTag == 0)
        {
            error = "WAV fmt chunk not found.";
            return false;
        }

        if (formatTag != 1)
        {
            error = $"Unsupported WAV format tag: {formatTag}.";
            return false;
        }

        if (channels <= 0)
        {
            error = "WAV channels are invalid.";
            return false;
        }

        if (sampleRate <= 0)
        {
            error = "WAV sample rate is invalid.";
            return false;
        }

        if (bitsPerSample != 16)
        {
            error = $"Unsupported WAV bits-per-sample: {bitsPerSample}.";
            return false;
        }

        if (pcmPayload.IsEmpty)
        {
            error = "WAV data chunk not found or empty.";
            return false;
        }

        audio = new PcmAudio
        {
            Data = new MemoryStream(pcmPayload.ToArray()),
            Format = new PcmFormatInfo(sampleRate, channels, PcmSampleFormat.S16LE)
        };
        return true;
    }
}
