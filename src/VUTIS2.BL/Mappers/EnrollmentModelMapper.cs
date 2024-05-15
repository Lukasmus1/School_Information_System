using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Mappers;

public class EnrollmentModelMapper : ModelMapperBase<EnrollmentEntity, EnrollmentListModel, EnrollmentDetailModel>, IEnrollmentModelMapper
{
    public override EnrollmentDetailModel MapToDetailModel(EnrollmentEntity? entity)
        => entity is null ? EnrollmentDetailModel.Empty : new EnrollmentDetailModel
        {
            Id = entity.Id,
            SubjectId = entity.SubjectId,
            StudentId = entity.StudentId
        };

    public override EnrollmentListModel MapToListModel(EnrollmentEntity? entity)
    => entity is null? EnrollmentListModel.Empty : new EnrollmentListModel
    {
        Id = entity.Id,
        SubjectId = entity.SubjectId,
        StudentId = entity.StudentId,
    };

    public override EnrollmentEntity MapToEntity (EnrollmentDetailModel enrollmentDetailModel) => new()
    {
        Id = enrollmentDetailModel.Id,
        SubjectId = enrollmentDetailModel.SubjectId,
        StudentId = enrollmentDetailModel.StudentId,
        Student = null,
        Subject = null
    };
}
