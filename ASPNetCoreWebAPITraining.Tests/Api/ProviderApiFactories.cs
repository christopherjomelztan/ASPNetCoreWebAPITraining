using ASPNetCoreWebAPITraining.Database;

namespace ASPNetCoreWebAPITraining.Tests.Api;

public sealed class SqlServerApiFactory : ProviderApiFactory
{
    protected override string Provider => StaticConfiguration.SqlServer;

    protected override string ConnectionString { get; } =
        $@"Server=(localdb)\MSSQLLocalDB;Database=TopeTraining_Test_{Guid.NewGuid():N};Trusted_Connection=True;TrustServerCertificate=True;";

    public override BaseDbContext CreateDbContext() =>
        new SqlServerDbContext(new DbContextOptionsBuilder<SqlServerDbContext>().UseSqlServer(ConnectionString).Options);
}

public sealed class MicrosoftAccessApiFactory : ProviderApiFactory
{
    protected override string Provider => StaticConfiguration.MicrosoftAccess;

    protected override string ConnectionString { get; } =
        $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Path.Combine(Path.GetTempPath(), $"TopeTraining_Test_{Guid.NewGuid():N}.accdb")};";

    public override BaseDbContext CreateDbContext() =>
        new MicrosoftAccessDbContext(new DbContextOptionsBuilder<MicrosoftAccessDbContext>().UseJet(ConnectionString).Options);
}

public sealed class SQLite3ApiFactory : ProviderApiFactory
{
    protected override string Provider => StaticConfiguration.SQLite3;

    protected override string ConnectionString { get; } =
        $"Data Source={Path.Combine(Path.GetTempPath(), $"TopeTraining_Test_{Guid.NewGuid():N}.db")};Pooling=False;";

    public override BaseDbContext CreateDbContext() =>
        new SQLite3DbContext(new DbContextOptionsBuilder<SQLite3DbContext>().UseSqlite(ConnectionString).Options);
}
