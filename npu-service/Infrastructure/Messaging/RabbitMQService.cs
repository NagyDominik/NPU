using System.Text;
using System.Text.Json;
using Domain.Npu;
using Microsoft.Extensions.Logging;
using Namespace.Infrastructure.Messaging;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(string hostName, string userName, string password, ILogger<RabbitMQService> logger)
    {
        _logger = logger;

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            // _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            // _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true);

            _logger.LogInformation("RabbitMQ connection established");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ");
            throw;
        }
    }

    public async Task PublishNpuCreatedEvent(Npu npu)
    {
        await PublishEvent(npu, MessageTypes.NpuCreated);
    }

    public async Task PublishNpuUpdatedEvent(Npu npu)
    {
        await PublishEvent(npu, MessageTypes.NpuUpdated);
    }

    private async Task PublishEvent<T>(T message, string eventType)
    {
        using var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(eventType, ExchangeType.Fanout, durable: true);
        try
        {
            var messageJson = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(exchange: eventType, routingKey: string.Empty, body: body);

            _logger.LogInformation("Event published to {EventType}: {MessageJson}", eventType, messageJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event to {EventType}", eventType);
        }
    }
}