namespace VUTIS2.BL.Models;

public record EnrollmentDetailModel : ModelBase
{
    public Guid SubjectId { get; init; }
    public SubjectListModel? Subject { get; init; }
    public Guid StudentId { get; init; }
    public StudentListModel? Student { get; init; }
    public static EnrollmentDetailModel Empty => new()
    {
        Id = Guid.Empty,
    };
}
