using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Facades;

public interface ISubjectFacade : IFacade<SubjectEntity, SubjectListModel, SubjectDetailModel>
{
    public Task<IEnumerable<SubjectListModel>> SearchAsync(string name);
    IEnumerable<SubjectListModel> GetOrderedByNameAsc(List<SubjectListModel> subjects);
    IEnumerable<SubjectListModel> GetOrderedByNameDesc(List<SubjectListModel> subjects);
    IEnumerable<SubjectListModel> GetOrderedByAbbreviationAsc(List<SubjectListModel> subjects);
    IEnumerable<SubjectListModel> GetOrderedByAbbreviationDesc(List<SubjectListModel> subjects);
}
