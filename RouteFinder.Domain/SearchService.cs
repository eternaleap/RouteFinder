using Microsoft.Extensions.Logging;

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

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (request.Filters!.OnlyCached.HasValue && request.Filters!.OnlyCached.Value)
        {
            var routes = await _cacheProviderSearchService.GetAsyns(request);
            
            if (routes == null)
            {
                _logger.LogWarning("The cache is empty");
                return new SearchResponse();
            }

            return GetResult(routes);
        }

        var routesCached = await _cacheProviderSearchService.GetAsyns(request);

        if (routesCached != null)
            return GetResult(routesCached);

        var responses = new List<Route>();

        foreach (var provider in _searchProviders)
        {
            if(await provider.IsAvailable())
                responses.AddRange(await provider.Search(request));
        }
        
        await _cacheProviderSearchService.SetAsync(request, responses.ToArray());

        return GetResult(responses);
    }

    private static SearchResponse GetResult(IReadOnlyCollection<Route> routes)
    {
        return new SearchResponse
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
        return true;
    }
}
