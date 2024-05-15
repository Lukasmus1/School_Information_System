namespace VUTIS2.BL.Models;

public record StudentListModel: ModelBase
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    public string? PhotoUrl { get; init; }
    public static StudentListModel Empty => new()
    {
        Id = Guid.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
    };
}
