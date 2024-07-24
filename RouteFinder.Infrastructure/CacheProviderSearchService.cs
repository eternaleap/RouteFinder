using RouteFinder.Domain;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Infrastructure;

public class CacheProviderSearchService : ICacheProviderSearchService
{
    private readonly ICacheRepository<SearchQuery, Route[]> _cacheRepository;

    public CacheProviderSearchService(ICacheRepository<SearchQuery, Route[]> cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public async Task<Route[]> GetAsyns(SearchQuery query)
    {
        return await _cacheRepository.GetAsync(query);
    }

    public async Task SetAsync(SearchQuery query, Route[] routes)
    {
        await _cacheRepository.SetAsync(query, routes, DateTimeOffset.Now.AddHours(1).TimeOfDay);
    }
}
