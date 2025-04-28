using Domain.Npu;

namespace Application.Services;

public interface INpuService
{
    Task<Npu> CreateNpu(CreateNpuInput input);
    Task<Npu?> UpdateNpu(Guid id, UpdateNpuInput input);
}