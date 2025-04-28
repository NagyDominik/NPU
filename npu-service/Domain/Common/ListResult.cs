namespace Domain.Common;

public record ListResult<T>
{
    public required IEnumerable<T> Items { get; init; }
}