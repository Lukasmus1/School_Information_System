using System.Collections.ObjectModel;
using VUTIS2.Common.Enums;

namespace VUTIS2.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }

    public required string RoomName { get; set; }

    public required ActivityType ActivityType { get; set; }
    public required string Description { get; set; }
    public Guid SubjectId { get; set; }
    public SubjectDetailModel? Subject { get; init; }
    public ObservableCollection<EvaluationListModel>? Evaluations { get; init; } = new();

    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        StartTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        RoomName = string.Empty,
        ActivityType = ActivityType.Empty,
        Description = string.Empty,
    };
}
