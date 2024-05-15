namespace VUTIS2.DAL.Migrator;

public interface IDbMigrator
{
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}
