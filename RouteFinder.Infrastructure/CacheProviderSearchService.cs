using RouteFinder.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace RouteFinder.Infrastructure;

public class CacheProviderSearchService : ICacheProviderSearchService
{
    private readonly IMemoryCache _cache;

    public CacheProviderSearchService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<Route[]> GetAsyns(SearchRequest request)
    {
        return _cache.Get<Route[]>(request);
    }

    public async Task SetAsync(SearchRequest request, Route[] routes)
    {
         _cache.Set(request, routes);
    }
}
