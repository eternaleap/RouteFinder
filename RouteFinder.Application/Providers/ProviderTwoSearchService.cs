using RouteFinder.Application.Gateways;
using RouteFinder.Domain;

namespace RouteFinder.Application.Providers;

public class ProviderTwoSearchService : IProviderSearchService
{
    private readonly IProviderTwoSearchGateway _providerTwoSearchGateway;

    public ProviderTwoSearchService(IProviderTwoSearchGateway providerTwoSearchGateway)
    {
        _providerTwoSearchGateway = providerTwoSearchGateway;
    }

    public async Task<IReadOnlyCollection<Route>> Search(SearchRequest request)
    {
        var routes = await _providerTwoSearchGateway.Search(new ProviderTwoSearchRequest
        {
            Departure = request.Origin,
            Arrival = request.Destination,
            DepartureDate = request.OriginDateTime,
            MinTimeLimit = request.Filters?.MinTimeLimit
        });
        
        return routes.Routes.Select(r => new Route
        {
            Destination = r.Arrival.Point,
            OriginDateTime = r.Departure.Date,
            TimeLimit = r.TimeLimit,
            DestinationDateTime = r.Arrival.Date,
            Origin = r.Departure.Point,
            Id = Guid.NewGuid(),
            Price = r.Price
        }).ToArray();
    }

    public async Task<bool> IsAvailable()
    {
        return await _providerTwoSearchGateway.IsAvailable();
    }
}
