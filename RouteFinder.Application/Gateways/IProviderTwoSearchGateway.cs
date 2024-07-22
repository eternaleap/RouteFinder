using RouteFinder.Application.Responses;

namespace RouteFinder.Application.Gateways;

public interface IProviderTwoSearchGateway
{
    public Task<ProviderTwoSearchResponse> Search(ProviderTwoSearchRequest request);

    public Task<bool> IsAvailable();
}
