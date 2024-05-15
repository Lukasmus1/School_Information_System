using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Facades;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<TListModel?> GetAsyncList(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> SaveAsync(TDetailModel model);
}
