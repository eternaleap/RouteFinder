using RouteFinder.Application.Responses;

namespace RouteFinder.Application.Gateways;

public interface IProviderOneSearchGateway
{
    public Task<ProviderOneSearchResponse> Search(ProviderOneSearchRequest request);

    public Task<bool> IsAvailable();
}
