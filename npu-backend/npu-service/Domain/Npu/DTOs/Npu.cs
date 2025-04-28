
namespace Domain.Npu;

public record NpuDto
{
    public required Guid ID { get; set; }
    public required string ElementName { get; set; }
    public required string Description { get; set; }
    public required Guid PictureID { get; set; }
    public required Guid CreatedBy { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}