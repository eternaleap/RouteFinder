using Mapster;
using Microsoft.AspNetCore.Mvc;
using RouteFinder.Api.Requests;
using RouteFinder.Api.Responses;
using RouteFinder.Domain;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController(ILogger<SearchController> logger, ISearchService searchService)
    : ControllerBase
{
    /// <summary>
    /// Поиск по входному фильтру
    /// </summary>
    /// <remarks>
    /// Working request example
    /// 
    ///     POST /Search
    ///     {
    ///         "origin": "Точка",
    ///         "destination": "Точка 2",
    ///         "originDateTime": "2021-07-23T07:00:48.638Z",
    ///         "filters": {
    ///             "destinationDateTime": "2024-07-23T07:00:48.638Z",
    ///             "maxPrice": 1000000000000000,
    ///             "minTimeLimit": "2028-07-23T07:00:48.638Z",
    ///             "onlyCached": false
    ///         }
    ///     }
    /// </remarks>
    [HttpPost(Name = "search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SearchResponse>> Search(SearchRequest query)
    {
        logger.LogInformation("Search request started");
        var result = await searchService.SearchAsync(query.Adapt<SearchQuery>(), default);

        if (result.Routes == null || result.Routes.Length == 0)
            return NotFound("No records found for such query");
        
        return Ok(result.Adapt<SearchResponse>());
    }
}
