using System.Reflection;

namespace DictationAssistant.App.ViewModels;

public class AboutWindowViewModel
{
    public string Title { get; } = "自动默写";

    public string Version { get; } = ResolveVersion();

    public string CopyrightNotice { get; } =
        "自动默写 v4（Avalonia）\n" +
        "\n" +
        "本版本为跨平台重构版本，目标是在 Linux/macOS/Windows 上提供一致的使用体验。\n" +
        "\n" +
        "Copyright (c) DictationAssistant Contributors";

    private static string ResolveVersion()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        return assembly.GetName().Version?.ToString() ?? "0.0.0";
    }
}
