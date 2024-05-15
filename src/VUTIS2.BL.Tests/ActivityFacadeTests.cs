using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;
using VUTIS2.Common.Enums;
using VUTIS2.Common.Tests;
using VUTIS2.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;
using Xunit.Abstractions;

namespace VUTIS2.BL.Tests;

public class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, evaluationFacade);
    }

    [Fact]
    public async Task CreateActivity_WithSubject_EqualsCreated()
    {
        var model = new ActivityDetailModel()
        {
            StartTime = DateTime.Parse("2023-01-04 07:00 AM"),
            EndTime = DateTime.Parse("2023-01-04 08:00 AM"),
            RoomName = "A100",
            ActivityType = ActivityType.Exam,
            Description = "Test activity description",
            SubjectId = SubjectSeeds.SampleSubject1.Id,
        };

        model = await _activityFacadeSUT.SaveAsync(model);
        var dbx = await DbContextFactory.CreateDbContextAsync();
        var entity = await dbx.Activities.SingleAsync(e => e.Id == model.Id);
        DeepAssert.Equal(model, ActivityModelMapper.MapToDetailModel(entity));
    }

    [Fact]
    public async Task CreateActivity_WithoutSubject_Throw()
    {
        var model = new ActivityDetailModel()
        {
            StartTime = DateTime.Parse("2023-01-04 07:00 AM"),
            EndTime = DateTime.Parse("2023-01-04 08:00 AM"),
            RoomName = "A100",
            ActivityType = ActivityType.Exam,
            Description = "Test activity description",
        };

        await Assert.ThrowsAsync<DbUpdateException>(async () => await _activityFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task GetAll_Single_SeededActivity()
    {
        var activities = await _activityFacadeSUT.GetAsync();
        var activity = activities.Single(i => i.Id == ActivitySeeds.SampleActivity1.Id);
        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.SampleActivity1), activity);
    }

    [Fact]
    public async Task GetById_SeededActivity()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.SampleActivity1.Id);
        var expected = ActivityModelMapper.MapToDetailModel(ActivitySeeds.SampleActivity1);
        DeepAssert.Equal(expected, activity);
    }

    [Fact]
    public async Task GetById_NonExistentActivity()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.EmptyActivity.Id);

        Assert.Null(activity);
    }

    [Fact]
    public async Task DeleteById_NonExistentActivity_Throw()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _activityFacadeSUT.DeleteAsync(ActivitySeeds.EmptyActivity.Id));
    }

    [Fact]
    public async Task DeleteById_SeededActivity()
    {
        await _activityFacadeSUT.DeleteAsync(ActivitySeeds.SampleActivity1.Id);

        var dbx = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbx.Activities.AnyAsync(i => i.Id == ActivitySeeds.SampleActivity1.Id));
    }
}
