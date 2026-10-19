using ASPNetCoreWebAPITraining.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNetCoreWebAPITraining.Tests.Database;

public class DatabaseRegistrationTests
{
    [Theory]
    [InlineData(StaticConfiguration.SqlServer, typeof(SqlServerDbContext))]
    [InlineData(StaticConfiguration.MicrosoftAccess, typeof(MicrosoftAccessDbContext))]
    [InlineData(StaticConfiguration.SQLite3, typeof(SQLite3DbContext))]
    public void BaseDbContext_ResolvesToTheContextOfTheConfiguredProvider(string provider, Type expectedContext)
    {
        using var serviceProvider = new ServiceCollection()
            .AddPersonDatabase(ConfigurationFor(provider))
            .BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        Assert.IsType(expectedContext, context);
    }

    [Fact]
    public void BaseDbContext_IsSharedWithinAScope()
    {
        using var serviceProvider = new ServiceCollection()
            .AddPersonDatabase(ConfigurationFor(StaticConfiguration.SQLite3))
            .BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var first = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        var second = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        Assert.Same(first, second);
    }

    [Theory]
    [InlineData("MySql")]
    [InlineData("")]
    [InlineData(null)]
    public void AddPersonDatabase_RejectsAnUnsupportedProviderAtStartup(string? provider)
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => new ServiceCollection().AddPersonDatabase(ConfigurationFor(provider)));

        Assert.Contains("Unsupported provider", exception.Message);
    }

    private static IConfiguration ConfigurationFor(string? provider) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Provider"] = provider,
                [$"ConnectionStrings:{StaticConfiguration.SqlServer}"] = @"Server=(localdb)\MSSQLLocalDB;Database=Unused;",
                [$"ConnectionStrings:{StaticConfiguration.MicrosoftAccess}"] = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=unused.accdb;",
                [$"ConnectionStrings:{StaticConfiguration.SQLite3}"] = "Data Source=unused.db;"
            })
            .Build();
}
