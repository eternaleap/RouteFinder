using MassTransit;
using Microsoft.Extensions.Logging;

namespace RouteFinder.RabbitMq.Consumers;

public class StringFaultConsumer : IConsumer<Fault<string>>
{
    private readonly ILogger<StringFaultConsumer> _logger;

    public StringFaultConsumer(ILogger<StringFaultConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<string>> context)
    {
        var error = context.Message.Exceptions[0];
        _logger.LogError(error.Message);
        return Task.CompletedTask;
    }
}