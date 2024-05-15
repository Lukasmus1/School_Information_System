using Microsoft.Extensions.DependencyInjection;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Mappers;

public class SubjectModelMapper(IEnrollmentModelMapper enrollmentModelMapper, IActivityModelMapper activityModelMapper) : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper
{

    public override SubjectListModel MapToListModel(SubjectEntity? entity)
        => entity is null
            ? SubjectListModel.Empty
            : new SubjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Abbreviation = entity.Abbreviation,
            };
    public override SubjectDetailModel MapToDetailModel(SubjectEntity? entity)
        => entity is null
            ? SubjectDetailModel.Empty
            : new SubjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Abbreviation = entity.Abbreviation,
                Enrollments = enrollmentModelMapper.MapToListModel(entity.Enrollments).ToObservableCollection(),
                Activities = activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection()
            };
    public override SubjectEntity MapToEntity(SubjectDetailModel model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Abbreviation = model.Abbreviation,
    };
}
