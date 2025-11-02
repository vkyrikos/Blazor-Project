using BlazorApp.Application.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace BlazorApp.Application.Cache;

public class AppCache(IMemoryCache cache) : ICache
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new(StringComparer.Ordinal);

    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(key, out T? cached) && cached is not null)
        {
            return cached;
        }

        var gate = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(cancellationToken);
        
        try
        {
            var value = await factory(cancellationToken);

            cache.Set(key, value!);
            return value!;
        }
        finally
        {
            gate.Release();
        }
    }

    public void Remove(string key) => cache.Remove(key);
}
