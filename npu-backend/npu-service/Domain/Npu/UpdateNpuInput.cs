
namespace Domain.Npu;
public record UpdateNpuInput
{
    public string? ElementName { get; set; }
    public string? Description { get; set; }
}