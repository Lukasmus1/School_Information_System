using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Mappers;

public class ActivityModelMapper(IEvaluationModelMapper evaluationModelMapper) : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel> , IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Description = entity.Description,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                RoomName = entity.RoomName,
                ActivityType = entity.ActivityType
            };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
    => entity is null ? ActivityDetailModel.Empty :  new ActivityDetailModel
    {
        Id = entity.Id,
        Description = entity.Description,
        StartTime = entity.StartTime,
        EndTime = entity.EndTime,
        RoomName = entity.RoomName,
        ActivityType = entity.ActivityType,
        SubjectId = entity.SubjectId,
        Evaluations = evaluationModelMapper.MapToListModel(entity.Evaluations).ToObservableCollection()
    };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)=> new()
    {
        Id = model.Id,
        Description = model.Description,
        StartTime = model.StartTime,
        EndTime = model.EndTime,
        RoomName = model.RoomName,
        ActivityType = model.ActivityType,
        SubjectId = model.SubjectId,
        Subject = null!,
    };
}
