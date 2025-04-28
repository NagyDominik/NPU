
namespace Domain.Npu;
public record CreateNpuInputDto
{
    public required string ElementName { get; set; }
    public required string Description { get; set; }
    public required Guid PictureID { get; set; }
}