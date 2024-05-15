using Microsoft.EntityFrameworkCore;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;
using VUTIS2.Common.Tests;
using VUTIS2.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace VUTIS2.BL.Tests;

public sealed class StudentFacadeTests : FacadeTestsBase
{
    private readonly IStudentFacade _studentFacadeSUT;

    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper, enrollmentFacade);
    }

    [Fact]
    public async Task Create_WithNonExistentItem_DoesNotThrow()
    {
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            FirstName = "Name",
            LastName = "Lastname",
        };

        await _studentFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task GetAll_Single_SeededStudent()
    {
        var students = await _studentFacadeSUT.GetAsync();
        var student = students.Single(i => i.Id == StudentSeeds.SampleStudent1.Id);
        DeepAssert.Equal(StudentModelMapper.MapToListModel(StudentSeeds.SampleStudent1), student);
    }

    [Fact]
    public async Task GetById_SeededStudent()
    {
        var student = await _studentFacadeSUT.GetAsync(StudentSeeds.SampleStudent1.Id);
        DeepAssert.Equal(StudentModelMapper.MapToDetailModel(StudentSeeds.SampleStudent1), student);
    }

    [Fact]
    public async Task GetById_NonExistentStudent()
    {
        var student = await _studentFacadeSUT.GetAsync(StudentSeeds.EmptyStudent.Id);

        Assert.Null(student);
    }

    [Fact]
    public async Task DeleteById_NonExistentStudent_Throw()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _studentFacadeSUT.DeleteAsync(StudentSeeds.EmptyStudent.Id));
    }

    [Fact]
    public async Task DeleteById_SeededStudent()
    {
        await _studentFacadeSUT.DeleteAsync(StudentSeeds.SampleStudent1.Id);

        var dbx = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbx.Students.AnyAsync(i => i.Id == StudentSeeds.SampleStudent1.Id));
    }

    [Fact]
    public async Task NewStudent_Insert_Added()
    {
        var student = new StudentDetailModel()
        {
            Id = Guid.Empty,
            FirstName = "Joe",
            LastName = "Biden",
        };

        student = await _studentFacadeSUT.SaveAsync(student);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var studentFromDb = await dbx.Students.SingleAsync(i => i.Id == student.Id);
        DeepAssert.Equal(student, StudentModelMapper.MapToDetailModel(studentFromDb));
    }
}
