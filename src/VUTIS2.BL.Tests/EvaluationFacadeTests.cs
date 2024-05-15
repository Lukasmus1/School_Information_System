using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.Common.Enums;
using VUTIS2.Common.Tests;
using VUTIS2.Common.Tests.Seeds;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.UnitOfWork;
using Xunit.Abstractions;

namespace VUTIS2.BL.Tests;

public class EvaluationFacadeTests : FacadeTestsBase
{
    private readonly IEvaluationFacade _evaluationFacadeSUT;

    public EvaluationFacadeTests(ITestOutputHelper output) : base(output)
    {
        _evaluationFacadeSUT = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
    }

    [Fact]
    public async Task CreateEvaluation_WithSubject_EqualsCreated()
    {
        var model = new EvaluationDetailModel()
        {
            Points = 1,
            ActivityId = ActivitySeeds.SampleActivity1.Id,
            StudentId = StudentSeeds.SampleStudent1.Id,
        };

        model = await _evaluationFacadeSUT.SaveAsync(model);
        var dbx = await DbContextFactory.CreateDbContextAsync();
        var entity = await dbx.Evaluations.SingleAsync(e => e.Id == model.Id);
        DeepAssert.Equal(model, EvaluationModelMapper.MapToDetailModel(entity));
    }

    [Fact]
    public async Task CreateEvaluation_WithoutStudent_Throw()
    {
        var model = new EvaluationDetailModel()
        {
            Points = 1,
            ActivityId = ActivitySeeds.SampleActivity1.Id,
            //StudentId = StudentSeeds.SampleStudent1.Id,
        };

        await Assert.ThrowsAsync<DbUpdateException>(async () => await _evaluationFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task GetAll_Single_SeededEvaluation()
    {
        var evaluations = await _evaluationFacadeSUT.GetAsync();
        var evaluation = evaluations.Single(i => i.Id == EvaluationSeeds.SampleEvaluation1.Id);
        DeepAssert.Equal(EvaluationModelMapper.MapToListModel(EvaluationSeeds.SampleEvaluation1 with {Student = null}), evaluation);
    }

    [Fact]
    public async Task GetById_SeededEvaluation()
    {
        var evaluation = await _evaluationFacadeSUT.GetAsync(EvaluationSeeds.SampleEvaluation1.Id);
        DeepAssert.Equal(EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.SampleEvaluation1), evaluation);
    }

    [Fact]
    public async Task GetById_NonExistentEvaluation()
    {
        var evaluation = await _evaluationFacadeSUT.GetAsync(EvaluationSeeds.EmptyEvaluation.Id);

        Assert.Null(evaluation);
    }

    [Fact]
    public async Task DeleteById_NonExistentEvaluation_Throw()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _evaluationFacadeSUT.DeleteAsync(EvaluationSeeds.EmptyEvaluation.Id));
    }

    [Fact]
    public async Task DeleteById_SeededEvaluation()
    {
        await _evaluationFacadeSUT.DeleteAsync(EvaluationSeeds.SampleEvaluation1.Id);

        var dbx = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbx.Evaluations.AnyAsync(i => i.Id == EvaluationSeeds.SampleEvaluation1.Id));
    }
}
