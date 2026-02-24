namespace DictationAssistant.App.Services;

public sealed class LocalTextFileService : ITextFileService
{
    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        return File.ReadAllTextAsync(path, cancellationToken);
    }

    public Task WriteAllTextAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(path, content, cancellationToken);
    }
}
