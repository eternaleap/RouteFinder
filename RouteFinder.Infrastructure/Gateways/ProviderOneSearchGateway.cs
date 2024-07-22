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
            Routes = _staticRoutes.Concat(ProviderOneRoutesCreator.CreateInstances(10)).ToArray()
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
