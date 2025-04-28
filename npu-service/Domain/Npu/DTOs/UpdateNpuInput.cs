
namespace Domain.Npu;
public record UpdateNpuInputDto
{
    public string? ElementName { get; set; }
    public string? Description { get; set; }
}