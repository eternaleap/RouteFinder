using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using RouteFinder.RabbitMq.Consumers;

namespace RouteFinder.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        Thread.Sleep(3*60*100);
        
        return services.AddMassTransit(x =>
        {
            x.AddConsumer<StringConsumer>();
            
            // have to connect to 5672 as it's default
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("amqp://guest:guest@rabbitmq/"), "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });    
                
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}