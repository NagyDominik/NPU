using Domain.Npu;

namespace Infrastructure.Messaging;

public interface IRabbitMQService
{
    Task PublishNpuCreatedEvent(Npu npu);
    Task PublishNpuUpdatedEvent(Npu npu);
}