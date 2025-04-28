
namespace Domain.Npu;
public record CreateNpuInput
{
    public required string ElementName { get; set; }
    public required string Description { get; set; }
    public required Guid PictureID { get; set; }
    public required Guid CreatedBy { get; set; }
}