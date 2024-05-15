namespace VUTIS2.DAL.Entities;

public record EnrollmentEntity : IEntity
{
    public Guid Id { get; set; }
    public required Guid StudentId { get; set; }
    public required Guid SubjectId { get; set; }
    public required StudentEntity? Student { get; init; }
    public required SubjectEntity? Subject { get; init; }
}
