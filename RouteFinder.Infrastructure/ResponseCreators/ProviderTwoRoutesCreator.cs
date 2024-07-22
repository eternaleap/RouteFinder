using RouteFinder.Application.Responses;

namespace RouteFinder.Infrastructure.ResponseCreators;

public class ProviderTwoRoutesCreator
{
    private static readonly Random Random = new Random();
    
    public static ProviderTwoRoute[] CreateInstances(int count)
    {
        var routes = new List<ProviderTwoRoute>();

        while (routes.Count <= count)
        {
            var route = new ProviderTwoRoute
            {
                Departure = new ProviderTwoPoint
                {
                    Point = $"Точка {Random.Next(100, 999)}", 
                    Date = DateTime.Now.AddDays(Random.Next(-180, 180)),
                },
                Arrival = new ProviderTwoPoint
                {
                    Point = $"Точка {Random.Next(1000, 9999)}",
                    Date = DateTime.Now.AddDays(Random.Next(-180, 180)),
                },
                Price = (decimal)Random.NextDouble() * 500,
                TimeLimit = DateTime.Now.AddHours(Random.Next(48, 72)),
            };

            routes.Add(route);
        }
        
        return routes.ToArray();
    }
}