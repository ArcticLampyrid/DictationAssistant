using DictationAssistant.App.Audio;
using ManagedBass;
using System.Diagnostics;
using System.IO;

namespace DictationAssistant.App.Audio;

public static class BassInitialization
{
    private static bool _initialized;
    private static readonly object _initLock = new();

    public static void EnsureInitialized()
    {
        lock (_initLock)
        {
            if (!_initialized)
            {
                if (!Bass.Init(0))
                {
                    throw new InvalidOperationException("Failed to initialize BASS audio decoder");
                }
                LoadPlugins();
                _initialized = true;
            }
        }
    }

    private static void LoadPlugins()
    {
        var baseDir = AppContext.BaseDirectory;
        var pluginDir = Path.Combine(baseDir, "bass_plugin");
        if (!Directory.Exists(pluginDir))
        {
            return;
        }

        try
        {
            foreach (var file in Directory.GetFiles(pluginDir))
            {
                var handle = Bass.PluginLoad(file);
                if (handle != 0)
                {
                    Trace.WriteLine($"[BassAudioDecoder] Loaded plugin: {Path.GetFileName(file)}");
                }
                else
                {
                    Trace.WriteLine($"[BassAudioDecoder] Failed to load plugin: {Path.GetFileName(file)} ({Bass.LastError})");
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Plugin loading error: {ex.Message}");
        }
    }
}
