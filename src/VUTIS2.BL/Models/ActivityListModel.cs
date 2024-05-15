using VUTIS2.Common.Enums;

namespace VUTIS2.BL.Models;

public record ActivityListModel :ModelBase
{
    public required string Description { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }

    public required string RoomName { get; init; }
    public required ActivityType ActivityType { get; init; }

    public static ActivityListModel Empty => new()
    {
        Id = Guid.Empty,
        StartTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        RoomName = String.Empty,
        ActivityType = ActivityType.Empty,
        Description = string.Empty,
    };
}
