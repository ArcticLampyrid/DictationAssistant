namespace DictationAssistant.App.Services;

public interface ITextFileService
{
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);

    Task WriteAllTextAsync(string path, string content, CancellationToken cancellationToken = default);
}
