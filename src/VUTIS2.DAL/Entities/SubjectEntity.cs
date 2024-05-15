namespace VUTIS2.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Abbreviation { get; set; }

        public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();

        public ICollection<EnrollmentEntity> Enrollments { get; init; } = new List<EnrollmentEntity>();
    }
}
