using DictationAssistant.App.Audio;
using ManagedBass;
using System.Diagnostics;
using System.IO;

namespace DictationAssistant.App.Audio;

public static class BassAudioDecoder
{
    private static bool _initialized;
    private static readonly object _initLock = new();

    public static bool EnsureInitialized()
    {
        lock (_initLock)
        {
            if (!_initialized)
            {
                if (!Bass.Init(0))
                {
                    Trace.WriteLine($"[BassAudioDecoder] Failed to initialize BASS: {Bass.LastError}");
                    return false;
                }
                LoadPlugins();
                _initialized = true;
            }
            return true;
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

    public static BassDecodeStream? DecodeFile(string filePath)
    {
        return CreateDecodeStream(filePath);
    }

    public static BassDecodeStream? DecodeStream(Stream inputStream)
    {
        return CreateDecodeStream(inputStream);
    }

    public static BassDecodeStream? CreateDecodeStream(string filePath)
    {
        try
        {
            if (!EnsureInitialized())
            {
                return null;
            }

            return BassDecodeStream.CreateFromFile(filePath);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Exception creating stream for {filePath}: {ex.Message}");
            return null;
        }
    }

    public static BassDecodeStream? CreateDecodeStream(Stream inputStream)
    {
        try
        {
            return BassDecodeStream.CreateFromStream(inputStream);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[BassAudioDecoder] Exception creating stream from Stream: {ex.Message}");
            return null;
        }
    }
}
