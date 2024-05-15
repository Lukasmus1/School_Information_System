using Microsoft.EntityFrameworkCore;
using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.Repositories;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL.Facades;

public class EvaluationFacade(IUnitOfWorkFactory unitOfWorkFactory, IEvaluationModelMapper evaluationModelMapper) : FacadeBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel, EvaluationEntityMapper>(unitOfWorkFactory, evaluationModelMapper), IEvaluationFacade
{
    protected override List<string> IncludesNavigationPathDetail => new()
    {
        $"{nameof(EvaluationEntity.Student)}",
        $"{nameof(EvaluationEntity.Activity)}"
    };
    public async Task<IEnumerable<EvaluationListModel>> GetAsyncFromActivity(Guid Id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<EvaluationEntity> query = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>().Get();
        foreach (string includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }
        List<EvaluationEntity> evaluations = await query.Where(e => e.ActivityId == Id).ToListAsync();
        return ModelMapper.MapToListModel(evaluations);
    }
    public IEnumerable<EvaluationListModel> GetOrderedByPointsAsc(List<EvaluationListModel> evaluations)
    {
        return evaluations.OrderBy(e => e.Points);
    }

    public IEnumerable<EvaluationListModel> GetOrderedByPointsDesc(List<EvaluationListModel> evaluations)
    {
        return evaluations.OrderByDescending(e => e.Points);
    }
}
