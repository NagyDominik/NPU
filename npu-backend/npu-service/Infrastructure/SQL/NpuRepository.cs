using Domain.Npu;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL;

public class NpuRepository(NpuDBContext context) : INpuRepository
{
    public async Task<List<Npu>> GetAllNpus()
    {
        return await context.Npus.ToListAsync();
    }

    public async Task<Npu?> GetNpuById(Guid id)
    {
        return await context.Npus.FindAsync(id);
    }

    public async Task<Npu> CreateNpu(CreateNpuInput input)
    {
        var npu = new Npu
        {
            ID = Guid.Empty, // This will be set by the database, set to obvious placeholder for easier debugging
            ElementName = input.ElementName,
            Description = input.Description,
            PictureID = input.PictureID,
            CreatedBy = input.CreatedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };

        context.Npus.Add(npu);
        await context.SaveChangesAsync();

        return npu;
    }

    public async Task<Npu?> UpdateNpu(Guid id, UpdateNpuInput input)
    {
        var existingNpu = await context.Npus.FindAsync(id);

        if (existingNpu == null)
        {
            return null;
        }
        if (input.ElementName != null)
        {
            existingNpu.ElementName = input.ElementName;
        }
        if (input.Description != null)
        {
            existingNpu.Description = input.Description;
        }

        await context.SaveChangesAsync();

        return existingNpu;
    }

    public async Task<bool> DeleteNpu(Guid id)
    {
        var npu = await context.Npus.FindAsync(id);

        if (npu == null)
        {
            return false;
        }

        context.Npus.Remove(npu);
        await context.SaveChangesAsync();

        return true;
    }
}