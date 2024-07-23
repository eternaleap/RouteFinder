using RouteFinder.Application;
using RouteFinder.Application.Gateways;
using RouteFinder.Application.Responses;
using RouteFinder.Infrastructure.ResponseCreators;

namespace RouteFinder.Infrastructure.Gateways;

public class ProviderTwoSearchGateway : IProviderTwoSearchGateway
{
    private readonly ProviderTwoRoute[] _staticRoutes;
    private static readonly Random Random = new ();
    
    public ProviderTwoSearchGateway()
    {
        _staticRoutes = ProviderTwoRoutesCreator.CreateInstances(50);
    }
    
    public async Task<ProviderTwoSearchResponse> Search(ProviderTwoSearchRequest request)
    {
        return new ProviderTwoSearchResponse
        {
            Routes = _staticRoutes.Concat(ProviderTwoRoutesCreator.CreateInstances(10))
                .Where(r => r.Departure.Point.Contains(request.Departure))
                .Where(r => r.Departure.Date > request.DepartureDate)
                .Where(r => r.Arrival.Point.Contains(request.Arrival))
                .Where(r => r.Arrival.Date > request.MinTimeLimit)
                .ToArray()
        };
    }

    public async Task<bool> IsAvailable()
    {
        int randomNumber = Random.Next(1, 33);

        if (randomNumber <= 5)
        {
            return true;
        }

        return false;
    }
}
