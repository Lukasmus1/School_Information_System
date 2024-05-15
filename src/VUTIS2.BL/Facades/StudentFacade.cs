using Microsoft.EntityFrameworkCore;
using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL.Facades;

public class StudentFacade(IUnitOfWorkFactory unitOfWorkFactory, IStudentModelMapper modelMapper, IEnrollmentFacade enrollmentFacade) : FacadeBase<StudentEntity, StudentListModel, StudentDetailModel, StudentEntityMapper>(unitOfWorkFactory, modelMapper), IStudentFacade
{
    public new async Task DeleteAsync(Guid id)
    {
        var Student = await GetAsync(id);
        if (Student is not null)
        {
            foreach (var enrollment in Student.Enrollments)
            {
                await enrollmentFacade.DeleteAsync(enrollment.Id);
            }

        }
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            await uow.GetRepository<StudentEntity, StudentEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public async Task<IEnumerable<StudentListModel>> SearchAsync(string name)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
        var students = await repository.Get().Where(s => s.FirstName.Contains(name) || s.LastName.Contains(name)).ToListAsync();
        return modelMapper.MapToListModel(students);
    }

    protected override List<string> IncludesNavigationPathDetail => new()
    {
        $"{nameof(StudentEntity.Enrollments)}"
    };
    public IEnumerable<StudentListModel> GetOrderedByLastNameAsc(List<StudentListModel> students)
    {
        return students.OrderBy(s => s.LastName);
    }

    public IEnumerable<StudentListModel> GetOrderedByLastNameDesc(List<StudentListModel> students)
    {
        return students.OrderByDescending(s => s.LastName);
    }

    public IEnumerable<StudentListModel> GetOrderedByFirstNameAsc(List<StudentListModel> students)
    {
        return students.OrderBy(s => s.FirstName);
    }

    public IEnumerable<StudentListModel> GetOrderedByFirstNameDesc(List<StudentListModel> students)
    {
        return students.OrderByDescending(s => s.FirstName);
    }
}
