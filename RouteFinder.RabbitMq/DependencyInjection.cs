using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using RouteFinder.RabbitMq.Consumers;

namespace RouteFinder.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        return services.AddMassTransit(x =>
        {
            x.AddConsumer<StringConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });    
                
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}