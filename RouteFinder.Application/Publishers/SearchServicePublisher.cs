using MassTransit;
using Microsoft.Extensions.Logging;
using RouteFinder.Domain;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Application.Publishers;

public class SearchServicePublisher : SearchService
{
    private readonly IBus _bus;
    
    public SearchServicePublisher(IBus bus, IEnumerable<IProviderSearchService> searchProviders, ICacheProviderSearchService cacheProviderSearchService, ILogger<SearchService> logger) : base(searchProviders, cacheProviderSearchService, logger)
    {
        _bus = bus;
    }

    public override async Task<SearchResult> SearchAsync(SearchQuery query, CancellationToken cancellationToken)
    {
        await _bus.Publish("search started", cancellationToken);
        var result = await base.SearchAsync(query, cancellationToken);
        await _bus.Publish("search performed", cancellationToken);
        return result;
    }
}