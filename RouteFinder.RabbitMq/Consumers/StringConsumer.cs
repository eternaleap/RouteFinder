using MassTransit;

namespace RouteFinder.RabbitMq.Consumers;

public class StringConsumer : IConsumer<string> 
{
    public Task Consume(ConsumeContext<string> context)
    {
        throw new Exception(context.Message);
    }
}