using RouteFinder.Domain.Entities;

namespace RouteFinder.Domain;

public interface ICacheProviderSearchService
{
    public Task<Route[]> GetAsyns(SearchQuery query);

    public Task SetAsync(SearchQuery query, Route[] routes);
}
