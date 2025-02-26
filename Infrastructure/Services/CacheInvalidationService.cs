using Microsoft.Extensions.Caching.Memory;
using Infrastructure.Extensions;
using Application.Abstractions.Authentication; // ✅ Import the extension method

namespace Infrastructure.Services;

public class CacheInvalidationService : ICacheInvalidationService
{
    private readonly IMemoryCache _cache;
    public CacheInvalidationService(IMemoryCache cache)
    {
        _cache = cache;
    }
    public void InvalidateByPrefix(string prefix)
    {
        foreach (var key in _cache.GetKeys()) 
        {
            if (key.StartsWith(prefix))
            {
                _cache.Remove(key);
            }
        }
    }
}
