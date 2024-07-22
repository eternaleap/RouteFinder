using RouteFinder.Application.Gateways;
using RouteFinder.Domain;

namespace RouteFinder.Application.Providers;

public class ProviderOneSearchService : IProviderSearchService
{
    private readonly IProviderOneSearchGateway _providerOneSearchGateway;

    public ProviderOneSearchService(IProviderOneSearchGateway providerOneSearchGateway)
    {
        _providerOneSearchGateway = providerOneSearchGateway;
    }

    public async Task<IReadOnlyCollection<Route>> Search(SearchRequest request)
    {
        var routes = await _providerOneSearchGateway.Search(new ProviderOneSearchRequest
        {
            MaxPrice = request.Filters.MaxPrice,
            From = request.Origin,
            To = request.Destination,
            DateFrom = request.OriginDateTime,
            DateTo = request.Filters.DestinationDateTime,
        });
        
        return routes.Routes.Select(r => new Route
        {
            Destination = r.To,
            OriginDateTime = r.DateFrom,
            TimeLimit = r.TimeLimit,
            DestinationDateTime = r.DateTo,
            Origin = r.From,
            Id = Guid.NewGuid(),
            Price = r.Price
        }).ToArray();
    }

    public async Task<bool> IsAvailable()
    {
        return await _providerOneSearchGateway.IsAvailable();
    }
}
