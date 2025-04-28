using Domain.Npu;

namespace Application;

public static class NpuMapper
{
    public static NpuDto MapToDto(Npu npu)
    {
        return new NpuDto
        {
            ID = npu.ID,
            ElementName = npu.ElementName,
            Description = npu.Description,
            PictureID = npu.PictureID,
            CreatedBy = npu.CreatedBy,
            CreatedAt = npu.CreatedAt
        };
    }

    public static CreateNpuInput MapToInternal(CreateNpuInputDto npuInput, Guid createdBy)
    {
        return new CreateNpuInput
        {
            ElementName = npuInput.ElementName,
            Description = npuInput.Description,
            PictureID = npuInput.PictureID,
            CreatedBy = createdBy
        };
    }

    public static UpdateNpuInput MapToInternal(UpdateNpuInputDto npuInput)
    {
        return new UpdateNpuInput
        {
            ElementName = npuInput.ElementName,
            Description = npuInput.Description,
        };
    }

}
