using RouteFinder.Domain;
using Microsoft.Extensions.Caching.Memory;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Infrastructure;

public class CacheProviderSearchService : ICacheProviderSearchService
{
    private readonly IMemoryCache _cache;

    public CacheProviderSearchService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<Route[]> GetAsyns(SearchQuery query)
    {
        return _cache.Get<Route[]>(query);
    }

    public async Task SetAsync(SearchQuery query, Route[] routes)
    {
        //Memory cache is thread safe, but for custom implementation I may use SemaphoreSlim .WaitAsync() and .Release(), but it's not required here
         _cache.Set(query, routes, DateTimeOffset.Now.AddHours(1));
    }
}
