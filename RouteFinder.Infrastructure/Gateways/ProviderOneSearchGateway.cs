using RouteFinder.Application;
using RouteFinder.Application.Gateways;
using RouteFinder.Application.Responses;
using RouteFinder.Infrastructure.ResponseCreators;

namespace RouteFinder.Infrastructure.Gateways;

public class ProviderOneSearchGateway : IProviderOneSearchGateway
{
    private readonly ProviderOneRoute[] _staticRoutes;
    private static readonly Random Random = new ();

    public ProviderOneSearchGateway()
    {
        _staticRoutes = ProviderOneRoutesCreator.CreateInstances(100);
    }

    public async Task<ProviderOneSearchResponse> Search(ProviderOneSearchRequest request)
    {
        return new ProviderOneSearchResponse
        {
            Routes = _staticRoutes.Concat(ProviderOneRoutesCreator.CreateInstances(10))
                .Where(r => r.From.Contains(request.From))
                .Where(r => r.To.Contains(request.To))
                .Where(r => r.Price < request.MaxPrice)
                .Where(r => r.TimeLimit > DateTime.Now)
                .Where(r => r.DateTo > request.DateTo)
                .Where(r => r.DateFrom > request.DateFrom)
                .ToArray()
        };
    }

    public async Task<bool> IsAvailable()
    {
        int randomNumber = Random.Next(1, 11);

        if (randomNumber <= 9)
        {
            return true;
        }

        return false;
    }
}
