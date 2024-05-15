using VUTIS2.BL.Mappers;
using VUTIS2.Common.Tests;
using VUTIS2.Common.Tests.Factories;
using VUTIS2.DAL;
using VUTIS2.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VUTIS2.BL.Facades;
using Xunit;
using Xunit.Abstractions;

namespace VUTIS2.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
        // DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
        DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        EnrollmentModelMapper = new EnrollmentModelMapper();
        StudentModelMapper = new StudentModelMapper(EnrollmentModelMapper);
        EvaluationModelMapper = new EvaluationModelMapper(StudentModelMapper);
        ActivityModelMapper = new ActivityModelMapper(EvaluationModelMapper);
        SubjectModelMapper = new SubjectModelMapper(EnrollmentModelMapper, ActivityModelMapper);
        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        enrollmentFacade = new EnrollmentFacade(UnitOfWorkFactory, EnrollmentModelMapper);
        evaluationFacade = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
        activityFacade = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, evaluationFacade);
        studentFacade = new StudentFacade(UnitOfWorkFactory, StudentModelMapper, enrollmentFacade);
        subjectFacade = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper, enrollmentFacade, activityFacade);

    }

    protected IDbContextFactory<SchoolDbContext> DbContextFactory { get; }

    protected StudentModelMapper StudentModelMapper{ get; }
    protected EvaluationModelMapper EvaluationModelMapper { get; }
    protected SubjectModelMapper SubjectModelMapper { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected EnrollmentModelMapper EnrollmentModelMapper { get; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    protected IEnrollmentFacade enrollmentFacade { get; }

    protected IActivityFacade activityFacade { get; }

    protected IStudentFacade studentFacade { get; }

    protected ISubjectFacade subjectFacade { get; }

    protected IEvaluationFacade evaluationFacade { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}
