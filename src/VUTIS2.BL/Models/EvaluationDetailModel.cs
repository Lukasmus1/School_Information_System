namespace VUTIS2.BL.Models;

public record EvaluationDetailModel : ModelBase
{
    public required int Points { get; init; }
    public string? Description { get; init; }
    public Guid ActivityId { get; set; }
    public ActivityListModel? Activity { get; init; }
    public Guid StudentId { get; set; }
    public StudentListModel? Student { get; init; }
    public static EvaluationDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Points = 0,
        Description = string.Empty,
        Activity = ActivityListModel.Empty,
        Student = StudentListModel.Empty
    };
}
