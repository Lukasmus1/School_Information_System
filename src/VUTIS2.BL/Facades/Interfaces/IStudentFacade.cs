using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Facades;

public interface IStudentFacade : IFacade<StudentEntity, StudentListModel, StudentDetailModel>
{
    public Task<IEnumerable<StudentListModel>> SearchAsync(string name);
    IEnumerable<StudentListModel> GetOrderedByLastNameAsc(List<StudentListModel> students);
    IEnumerable<StudentListModel> GetOrderedByLastNameDesc(List<StudentListModel> students);
    IEnumerable<StudentListModel> GetOrderedByFirstNameAsc(List<StudentListModel> students);
    IEnumerable<StudentListModel> GetOrderedByFirstNameDesc(List<StudentListModel> students);
}
