namespace RouteFinder.Domain;

public interface ICacheProviderSearchService
{
    public Task<Route[]> GetAsyns(SearchRequest request);

    public Task SetAsync(SearchRequest request, Route[] routes);
}
