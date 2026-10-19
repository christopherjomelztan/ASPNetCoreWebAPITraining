using ASPNetCoreWebAPITraining.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ASPNetCoreWebAPITraining.Tests.Api;

/// <summary>
/// Hosts the real API against a throwaway database of one provider. The database is created
/// from the provider's migrations (so the seed data is exercised) and dropped on disposal.
/// </summary>
public abstract class ProviderApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected abstract string Provider { get; }

    protected abstract string ConnectionString { get; }

    public abstract BaseDbContext CreateDbContext();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Provider", Provider);
        builder.UseSetting($"ConnectionStrings:{Provider}", ConnectionString);
    }

    public async Task InitializeAsync()
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await using var context = CreateDbContext();
        await context.Database.EnsureDeletedAsync();
    }
}
