using DictationAssistant.Core.Abstractions;
using DictationAssistant.Core.Audio;
using DictationAssistant.Core.Models;

namespace DictationAssistant.Core.Services;

public abstract class CachedVoice : IPreloadableVoice
{
    private readonly int _cacheCapacity;
    private readonly LinkedList<CacheEntry> _cacheOrder = new();
    private readonly Dictionary<CacheKey, LinkedListNode<CacheEntry>> _cacheMap = new();
    private readonly object _lock = new();

    protected CachedVoice(int cacheCapacity = 4)
    {
        _cacheCapacity = cacheCapacity;
    }

    public abstract string Name { get; }

    protected abstract Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);

    public async Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        var key = new CacheKey(text, options.Rate ?? 0);
        lock (_lock)
        {
            if (TryGetFromCache(key, out var cached))
            {
                return cached;
            }
        }

        var result = await SynthesizePcmDirectAsync(text, options, ct).ConfigureAwait(false);
        if (result is not null)
        {
            lock (_lock)
            {
                PutInCache(key, result);
            }
        }

        return result;
    }

    public async Task PreloadAsync(string text, VoiceSynthesisOptions options, CancellationToken ct)
    {
        var key = new CacheKey(text, options.Rate ?? 0);
        lock (_lock)
        {
            if (_cacheMap.ContainsKey(key))
            {
                return;
            }
        }

        try
        {
            var result = await SynthesizePcmDirectAsync(text, options, ct).ConfigureAwait(false);
            if (result is not null)
            {
                lock (_lock)
                {
                    PutInCache(key, result);
                }
            }
        }
        catch
        {
        }
    }

    private bool TryGetFromCache(CacheKey key, out PcmAudio? audio)
    {
        if (_cacheMap.TryGetValue(key, out var node))
        {
            _cacheOrder.Remove(node);
            _cacheOrder.AddFirst(node);
            audio = node.Value.Audio;
            return true;
        }

        audio = null;
        return false;
    }

    private void PutInCache(CacheKey key, PcmAudio audio)
    {
        if (_cacheOrder.Count >= _cacheCapacity)
        {
            var lastNode = _cacheOrder.Last;
            if (lastNode is not null)
            {
                _cacheOrder.RemoveLast();
                _cacheMap.Remove(lastNode.Value.Key);
            }
        }

        var newNode = new LinkedListNode<CacheEntry>(new CacheEntry { Key = key, Audio = audio });
        _cacheOrder.AddFirst(newNode);
        _cacheMap[key] = newNode;
    }

    private readonly record struct CacheKey(string Text, int Rate);

    private sealed class CacheEntry
    {
        public required CacheKey Key { get; init; }
        public required PcmAudio Audio { get; init; }
    }
}
