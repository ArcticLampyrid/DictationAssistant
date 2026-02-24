using DictationAssistant.Core.Abstractions;

namespace DictationAssistant.App.Services.Tts;

public sealed class LinuxPico2WaveTtsEngine : ITtsEngine
{
    private readonly string _playbackCommand;

    public LinuxPico2WaveTtsEngine(string playbackCommand)
    {
        _playbackCommand = playbackCommand;
    }

    public string Name => "Linux pico2wave";

    public async Task SpeakAsync(string text, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-{Guid.NewGuid():N}.wav");
        try
        {
            await ProcessRunner.RunAsync("pico2wave", ["-w", tempFile, text], null, cancellationToken).ConfigureAwait(false);
            await ProcessRunner.RunAsync(_playbackCommand, [tempFile], null, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    public async Task<byte[]?> SynthesizeAudioAsync(string text, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"dictationassistant-{Guid.NewGuid():N}.wav");
        try
        {
            await ProcessRunner.RunAsync("pico2wave", ["-w", tempFile, text], null, cancellationToken).ConfigureAwait(false);
            return await File.ReadAllBytesAsync(tempFile, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
