using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.Common.Enums;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.Repositories;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL.Facades;

public class ActivityFacade(IUnitOfWorkFactory unitOfWorkFactory, IActivityModelMapper activityModelMapper, IEvaluationFacade evaluationFacade)
    : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>(unitOfWorkFactory,
        activityModelMapper), IActivityFacade
{
    public new async Task DeleteAsync(Guid id)
    {
        var Activity = await GetAsync(id);
        if (Activity is not null)
        {
            if (Activity.Evaluations is not null)
            {
                foreach (var evaluation in Activity.Evaluations)
                {
                    await evaluationFacade.DeleteAsync(evaluation.Id);

                }
            }
        }
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            await uow.GetRepository<ActivityEntity, ActivityEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }
    public async Task<IEnumerable<ActivityListModel>> GetAsyncFromSubject(Guid Id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        List<ActivityEntity> activities = await repository.Get().Where(a => a.SubjectId == Id).ToListAsync();
        return ModelMapper.MapToListModel(activities);
    }
    protected override List<string> IncludesNavigationPathDetail => new()
    {
        $"{nameof(ActivityEntity.Evaluations)}.{nameof(EvaluationEntity.Student)}"
    };

    public new async Task<ActivityDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        foreach (string includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }

        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public async Task<IEnumerable<ActivityListModel>> GetActivitiesStartTime(DateTime startTime, bool from,
        Guid SubjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        // Filter and sort the activities
        List<ActivityEntity> activities = await repository
            .Get()
            .Where(a => from ? a.StartTime >= startTime : a.StartTime <= startTime)
            .Where(a => a.SubjectId == SubjectId)
            .ToListAsync();
        return ModelMapper.MapToListModel(activities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetActivitiesEndTime(DateTime endTime, bool from, Guid SubjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        // Filter and sort the activities
        List<ActivityEntity> activities = await repository
            .Get()
            .Where(a => from ? a.EndTime >= endTime : a.EndTime <= endTime)
            .Where(a => a.SubjectId == SubjectId)
            .ToListAsync();
        return ModelMapper.MapToListModel(activities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetActivitiesByBoth(DateTime startTime, bool startFrom, DateTime endTime, bool endFrom,
        Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        // Filter and sort the activities
        List<ActivityEntity> activities = await repository
            .Get()
            .Where(a => a.SubjectId == subjectId)
            .Where(a => endFrom ? a.EndTime >= endTime : a.EndTime <= endTime && startFrom ? a.StartTime >= startTime : a.StartTime <= startTime)
            .ToListAsync();
        return ModelMapper.MapToListModel(activities);
    }

    public IEnumerable<ActivityListModel> GetOrderedByStartTimeAsc(IEnumerable<ActivityListModel> activities)
    {
        return activities.OrderBy(a => a.StartTime);
    }

    public IEnumerable<ActivityListModel> GetOrderedByStartTimeDesc(IEnumerable<ActivityListModel> activities)
    {
        return activities.OrderByDescending(a => a.StartTime);
    }

    public IEnumerable<ActivityListModel> GetOrderedByEndTimeAsc(IEnumerable<ActivityListModel> activities)
    {
        return activities.OrderBy(a => a.EndTime);
    }

    public IEnumerable<ActivityListModel> GetOrderedByEndTimeDesc(IEnumerable<ActivityListModel> activities)
    {
        return activities.OrderByDescending(a => a.EndTime);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncBySubject(Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        // Filter and sort the activities
        List<ActivityEntity> activities = await repository
            .Get()
            .Where(a => a.SubjectId == subjectId)
            .ToListAsync();
        return ModelMapper.MapToListModel(activities);
    }
}
