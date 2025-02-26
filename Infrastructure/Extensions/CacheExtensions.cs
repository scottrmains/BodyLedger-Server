using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions;

public static class CacheExtensions
{
    public static IEnumerable<string> GetKeys(this IMemoryCache cache)
    {
        if (cache is MemoryCache memoryCache)
        {
            var field = typeof(MemoryCache).GetField("_entries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var entries = field?.GetValue(memoryCache) as ConcurrentDictionary<object, object>;
            return entries?.Keys.OfType<string>() ?? Enumerable.Empty<string>();
        }
        return Enumerable.Empty<string>();
    }
}
