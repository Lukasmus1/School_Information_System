using VUTIS2.DAL.Entities;

namespace VUTIS2.DAL.Mappers;

public class EnrollmentEntityMapper : IEntityMapper<EnrollmentEntity>
{
    public void MapToExistingEntity(EnrollmentEntity existingEntity, EnrollmentEntity newEntity)
    {
        existingEntity.SubjectId = newEntity.SubjectId;
        existingEntity.StudentId = newEntity.StudentId;
    }
}
