using Microsoft.Extensions.Logging;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Domain;

public class SearchService : ISearchService
{
    private readonly IEnumerable<IProviderSearchService> _searchProviders;
    private readonly ICacheProviderSearchService _cacheProviderSearchService;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IEnumerable<IProviderSearchService> searchProviders, ICacheProviderSearchService cacheProviderSearchService, ILogger<SearchService> logger)
    {
        _searchProviders = searchProviders;
        _cacheProviderSearchService = cacheProviderSearchService;
        _logger = logger;
    }

    public async Task<SearchResult> SearchAsync(SearchQuery query, CancellationToken cancellationToken)
    {
        if (query.Filters != null && query.Filters!.OnlyCached.HasValue && query.Filters!.OnlyCached.Value)
        {
            var routes = await _cacheProviderSearchService.GetAsyns(query);
            
            if (routes == null)
            {
                _logger.LogWarning("The cache is empty");
                return new SearchResult();
            }

            return GetResult(routes);
        }

        var routesCached = await _cacheProviderSearchService.GetAsyns(query);

        if (routesCached != null && routesCached.Length > 0)
            return GetResult(routesCached);

        var routesFromExternalServices = new List<Route>();

        foreach (var provider in _searchProviders)
        {
            if(await provider.IsAvailable())
                routesFromExternalServices.AddRange(await provider.Search(query));
            else
                _logger.LogError($"{provider.GetType()} is unavailable, skipping it");
        }
        
        await _cacheProviderSearchService.SetAsync(query, routesFromExternalServices.ToArray());

        return GetResult(routesFromExternalServices);
    }

    private static SearchResult GetResult(IReadOnlyCollection<Route> routes)
    {
        if (routes.Count == 0)
            return new SearchResult { Routes = routes.ToArray()};
        
        return new SearchResult
        {
            Routes = routes.ToArray(),
            MaxPrice = routes.Max(r => r.Price),
            MinPrice = routes.Min(r => r.Price),
            MaxMinutesRoute = routes.Max(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes),
            MinMinutesRoute = routes.Min(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes)
        };
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        return _searchProviders.Any(sp => sp.IsAvailable().Result);
    }
}
