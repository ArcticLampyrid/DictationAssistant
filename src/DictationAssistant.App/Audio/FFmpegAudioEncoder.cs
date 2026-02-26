using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DictationAssistant.App.Audio;

public sealed class FFmpegAudioEncoder : IAsyncDisposable
{
    private Process? _process;
    private readonly Stream _inputStream;
    private bool _disposed;

    public static async Task<bool> IsFFmpegAvailableAsync()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(startInfo);
            if (process == null) return false;
            await process.WaitForExitAsync().ConfigureAwait(false);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public FFmpegAudioEncoder(string outputPath, int sampleRate, int channels, string format)
    {
        var arguments = GetFFmpegArguments(outputPath, sampleRate, channels, format);

        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = false,
                RedirectStandardError = true
            }
        };

        _process.Start();
        _inputStream = _process.StandardInput.BaseStream;

        WriteWavHeader(_inputStream, sampleRate, channels, 0);
    }

    private static void WriteWavHeader(Stream output, int sampleRate, int channels, int dataLength)
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

    private static string GetFFmpegArguments(string outputPath, int sampleRate, int channels, string format)
    {
        var formatLower = format.ToLowerInvariant();
        return formatLower switch
        {
            "mp3" => $"-f wav -ar {sampleRate} -ac {channels} -i pipe:0 -codec:a libmp3lame -q:a 2 -y \"{outputPath}\"",
            "opus" => $"-f wav -ar {sampleRate} -ac {channels} -i pipe:0 -codec:a libopus -b:a 128k -y \"{outputPath}\"",
            _ => throw new NotSupportedException($"Format '{format}' is not supported")
        };
    }

    public Stream InputStream => _inputStream;

    public async Task FinishAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed) return;

        await _inputStream.FlushAsync(cancellationToken).ConfigureAwait(false);
        _inputStream.Close();

        if (_process != null)
        {
            await _process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            if (_process.ExitCode != 0)
            {
                var error = await _process.StandardError.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
                throw new InvalidOperationException($"FFmpeg encoding failed: {error}");
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        try
        {
            _inputStream.Close();
        }
        catch
        {
        }

        if (_process != null)
        {
            try
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                }
            }
            catch
            {
            }

            _process.Dispose();
            _process = null;
        }
    }
}
