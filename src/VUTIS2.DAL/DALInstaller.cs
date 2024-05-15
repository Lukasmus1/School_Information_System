using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VUTIS2.DAL.Factories;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.Migrator;
using VUTIS2.DAL.Options;

namespace VUTIS2.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, DALOptions options)
    {
        services.AddSingleton(options);

        if (options is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (string.IsNullOrEmpty(options.DatabaseDirectory))
        {
            throw new InvalidOperationException($"{nameof(options.DatabaseDirectory)} is not set");
        }
        if (string.IsNullOrEmpty(options.DatabaseName))
        {
            throw new InvalidOperationException($"{nameof(options.DatabaseName)} is not set");
        }

        services.AddSingleton<IDbContextFactory<SchoolDbContext>>(_ =>
            new DbContextSqLiteFactory(options.DatabaseFilePath, options?.SeedDemoData ?? false));
        services.AddSingleton<IDbMigrator, DbMigrator>();
        services.AddSingleton<EnrollmentEntityMapper>();
        services.AddSingleton<StudentEntityMapper>();
        services.AddSingleton<ActivityEntityMapper>();
        services.AddSingleton<SubjectEntityMapper>();
        services.AddSingleton<EvaluationEntityMapper>();


        return services;
    }
}
