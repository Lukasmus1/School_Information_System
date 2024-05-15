namespace VUTIS2.BL.Models;

public record EvaluationListModel : ModelBase
{
    public required int Points { get; init; }
    public string? Description { get; init; }

    public required Guid StudentId { get; init; }
    public required StudentListModel? Student { get; init; }
    public static EvaluationListModel Empty => new()
    {
        Id = Guid.Empty,
        Points = 0,
        Description = string.Empty,
        Student = StudentListModel.Empty,
        StudentId = Guid.Empty
    };
}
