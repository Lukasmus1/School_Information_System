namespace VUTIS2.DAL.Entities
{
    public record StudentEntity : IEntity
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string? PhotoUrl { get; set; }

        public ICollection<EnrollmentEntity> Enrollments { get; init; } = new List<EnrollmentEntity>();
    }
}
