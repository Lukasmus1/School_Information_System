using Microsoft.EntityFrameworkCore;

namespace VUTIS2.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<SchoolDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<SchoolDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedDemoData)
    {
        _seedDemoData = seedDemoData;
        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public SchoolDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
}
