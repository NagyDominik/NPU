using Domain.Npu;
using Infrastructure.Messaging;
using Infrastructure.SQL;

namespace Application.Services;

public class NpuService(INpuRepository npuRepository, IRabbitMQService rabbitMqService) : INpuService
{
    public async Task<Npu> CreateNpu(CreateNpuInput input)
    {
        var createdNpu = await npuRepository.CreateNpu(input);

        // Publish event after successful creation
        await rabbitMqService.PublishNpuCreatedEvent(createdNpu);

        return createdNpu;
    }

    public async Task<Npu?> UpdateNpu(Guid id, UpdateNpuInput input)
    {
        var updatedNpu = await npuRepository.UpdateNpu(id, input);

        // Publish event after successful update if NPU was found
        if (updatedNpu != null)
        {
            await rabbitMqService.PublishNpuUpdatedEvent(updatedNpu);
        }

        return updatedNpu;
    }
}