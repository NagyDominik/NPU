using Domain.Npu;

namespace Infrastructure.SQL;

public interface INpuRepository
{
    public Task<List<Npu>> GetAllNpus();
    public Task<Npu?> GetNpuById(Guid id);
    public Task<Npu> CreateNpu(CreateNpuInput npu);
    public Task<Npu?> UpdateNpu(Guid id, UpdateNpuInput updatedNpu);
    public Task<bool> DeleteNpu(Guid id);
}