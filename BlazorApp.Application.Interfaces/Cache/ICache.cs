using Microsoft.Extensions.Caching.Memory;

namespace BlazorApp.Application.Interfaces.Cache;

public interface ICache
{
    Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken ct = default);

    void Remove(string key);
}
