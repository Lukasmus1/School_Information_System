using Microsoft.EntityFrameworkCore;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;
using VUTIS2.Common.Tests;
using VUTIS2.Common.Tests.Seeds;
using VUTIS2.DAL.Entities;
using Xunit.Abstractions;

namespace VUTIS2.BL.Tests;

public class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSUT;

    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper,  enrollmentFacade, activityFacade);
    }

    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            Name = "Sub",
            Abbreviation = "Abbr",
        };

        await _subjectFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task GetAll_Single_SeededSubject()
    {
        var subjects = await _subjectFacadeSUT.GetAsync();
        var subject = subjects.Single(i => i.Id == SubjectSeeds.SampleSubject1.Id);
        DeepAssert.Equal(SubjectModelMapper.MapToListModel(SubjectSeeds.SampleSubject1), subject);
    }

    [Fact]
    public async Task GetById_SeededSubject()
    {
        var subject = await _subjectFacadeSUT.GetAsync(SubjectSeeds.SampleSubject1.Id);
        var expected = SubjectModelMapper.MapToDetailModel(SubjectSeeds.SampleSubject1);
        DeepAssert.Equal(expected, subject);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var subject = await _subjectFacadeSUT.GetAsync(SubjectSeeds.EmptySubject.Id);

        Assert.Null(subject);
    }

    [Fact]
    public async Task DeleteById_NonExistent_Throw()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.EmptySubject.Id));
    }

    [Fact]
    public async Task DeleteById_Seeded()
    {
        await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.SampleSubject1.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbx.Subjects.AnyAsync(i => i.Id == SubjectSeeds.SampleSubject1.Id));
    }

    [Fact]
    public async Task NewSubject_InsertOrUpdate_Added()
    {
        var subject = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            Name = "Matematicka Analyza 1",
            Abbreviation = "IMA1",
        };

        subject = await _subjectFacadeSUT.SaveAsync(subject);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var subjectFromDb = await dbx.Subjects.SingleAsync(i => i.Id == subject.Id);
        DeepAssert.Equal(subject, SubjectModelMapper.MapToDetailModel(subjectFromDb));
    }

    [Fact]
    public async Task SeededSubject_InsertOrUpdate_Updated()
    {
        var subject = new SubjectDetailModel()
        {
            Id = SubjectSeeds.SampleSubject1.Id,
            Name = SubjectSeeds.SampleSubject1.Name,
            Abbreviation = SubjectSeeds.SampleSubject1.Abbreviation,
        };

        subject.Name += "updated";
        subject.Abbreviation += "updated";

        subject = await _subjectFacadeSUT.SaveAsync(subject);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var subjectFrombDb = await dbx.Subjects.SingleAsync(i => i.Id == subject.Id);
        DeepAssert.Equal(subject, SubjectModelMapper.MapToDetailModel(subjectFrombDb));
    }
}
