using Microsoft.EntityFrameworkCore;
using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL.Facades;

public class SubjectFacade(IUnitOfWorkFactory unitOfWorkFactory, ISubjectModelMapper modelMapper, IEnrollmentFacade enrollmentFacade, IActivityFacade activityFacade) : FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>(unitOfWorkFactory, modelMapper), ISubjectFacade
{
    protected override List<string> IncludesNavigationPathDetail => new()
    {
        $"{nameof(SubjectEntity.Enrollments)}",
        $"{nameof(SubjectEntity.Activities)}"
    };

    public new async Task DeleteAsync(Guid id)
    {
        var Subject = await GetAsync(id);
        if (Subject is not null)
        {
            foreach (var enrollment in Subject.Enrollments)
            {
                await enrollmentFacade.DeleteAsync(enrollment.Id);
            }
            foreach (var activity in Subject.Activities)
            {
                await activityFacade.DeleteAsync(activity.Id);
            }

        }
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            await uow.GetRepository<SubjectEntity, SubjectEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }
    public async Task<IEnumerable<SubjectListModel>> SearchAsync(string name)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();
        var subjects = await repository.Get().Where(s => s.Name.Contains(name) || s.Abbreviation.Contains(name)).ToListAsync();
        return modelMapper.MapToListModel(subjects);
    }
    public IEnumerable<SubjectListModel> GetOrderedByNameAsc(List<SubjectListModel> subjects)
    {
        return subjects.OrderBy(s => s.Name);
    }

    public IEnumerable<SubjectListModel> GetOrderedByNameDesc(List<SubjectListModel> subjects)
    {
        return subjects.OrderByDescending(s => s.Name);
    }

    public IEnumerable<SubjectListModel> GetOrderedByAbbreviationAsc(List<SubjectListModel> subjects)
    {
        return subjects.OrderBy(s => s.Abbreviation);
    }

    public IEnumerable<SubjectListModel> GetOrderedByAbbreviationDesc(List<SubjectListModel> subjects)
    {
        return subjects.OrderByDescending(s => s.Abbreviation);
    }
}
