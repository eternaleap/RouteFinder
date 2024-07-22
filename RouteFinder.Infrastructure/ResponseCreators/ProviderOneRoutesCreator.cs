using RouteFinder.Application.Responses;

namespace RouteFinder.Infrastructure.ResponseCreators;

public class ProviderOneRoutesCreator
{
    private static readonly Random Random = new Random();

    public static ProviderOneRoute[] CreateInstances(int count)
    {
        var routes = new List<ProviderOneRoute>();

        while (routes.Count <= count)
        {
            var route = new ProviderOneRoute
            {
                From = $"Точка {Random.Next(100, 999)}", 
                To = $"Точка {Random.Next(1000, 9999)}", 
                DateFrom = DateTime.Now.AddDays(Random.Next(-180, 180)), 
                DateTo = DateTime.Now.AddDays(Random.Next(-180, 180)), 
                Price = (decimal)Random.NextDouble() * 500, 
                TimeLimit = DateTime.Now.AddHours(Random.Next(48, 72)), 
            };
            
            routes.Add(route);
        }

        return routes.ToArray();;
    }
}