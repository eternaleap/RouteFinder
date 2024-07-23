using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RouteFinder.Domain;

namespace RouteFinder.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController(ISearchService searchService)
    : ControllerBase
{
    [HttpGet(Name = "ping")]
    public async Task<IActionResult> IsAvailable()
    {
        var isAvailable = await searchService.IsAvailableAsync(default);

        if (isAvailable)
            return Ok();
        else return Problem("Search services are unavailable. Please investigate logs for details");
    }
}
