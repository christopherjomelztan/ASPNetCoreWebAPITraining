using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public static class DatabaseServiceCollectionExtensions
  {
    public static IServiceCollection AddPersonDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      var configuredContext = ContextTypeFor(configuration[StaticConfiguration.ProviderSetting]);

      services.AddDbContext<SqlServerDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString(StaticConfiguration.SqlServer)));
      services.AddDbContext<SQLite3DbContext>(options =>
          options.UseSqlite(configuration.GetConnectionString(StaticConfiguration.SQLite3)));
      services.AddDbContext<MicrosoftAccessDbContext>(options =>
          options.UseJet(configuration.GetConnectionString(StaticConfiguration.MicrosoftAccess)));

      return services.AddScoped(serviceProvider => (BaseDbContext)serviceProvider.GetRequiredService(configuredContext));
    }

    private static Type ContextTypeFor(string? provider) => provider switch
    {
      StaticConfiguration.SqlServer => typeof(SqlServerDbContext),
      StaticConfiguration.SQLite3 => typeof(SQLite3DbContext),
      StaticConfiguration.MicrosoftAccess => typeof(MicrosoftAccessDbContext),
      _ => throw new InvalidOperationException($"Unsupported provider: '{provider}'.")
    };
  }
}
