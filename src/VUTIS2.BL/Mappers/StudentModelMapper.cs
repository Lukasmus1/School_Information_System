using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Mappers;

public class StudentModelMapper : ModelMapperBase<StudentEntity, StudentListModel, StudentDetailModel>, IStudentModelMapper
{
    private IEnrollmentModelMapper _enrollmentModelMapper;

    public StudentModelMapper(IEnrollmentModelMapper enrollmentModelMapper)
    {
        _enrollmentModelMapper = enrollmentModelMapper;
    }

    public override StudentListModel MapToListModel(StudentEntity? entity)
    => entity is null ? StudentListModel.Empty : new StudentListModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PhotoUrl = entity.PhotoUrl,
        };

    public override StudentDetailModel MapToDetailModel(StudentEntity? entity)
        => entity is null
            ? StudentDetailModel.Empty
            : new StudentDetailModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PhotoUrl = entity.PhotoUrl,
                Enrollments = new ObservableCollection<EnrollmentListModel>(_enrollmentModelMapper.MapToListModel(entity.Enrollments)),
            };
    public override StudentEntity MapToEntity(StudentDetailModel model)
        => new()
    {
        Id = model.Id,
        FirstName = model.FirstName,
        LastName = model.LastName,
        PhotoUrl = model.PhotoUrl,
    };
}
