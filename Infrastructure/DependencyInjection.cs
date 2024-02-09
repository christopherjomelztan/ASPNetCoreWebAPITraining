using Domain.Common;
using Infrastructure.Abstractions;
using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, string provider)
    {
        services.AddDbContext<MySqlDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString(StaticConfiguration.MySql),
                new MySqlServerVersion(StaticConfiguration.MySqlVersion))
        );

        services.AddDbContext<SqlServerDbContext>(options =>
            options.UseSqlServer(configuration
                .GetConnectionString(StaticConfiguration.SqlServer))
        );

        services.AddTransient<IDbContextFactory>(_ => provider switch
        {
            StaticConfiguration.MySql => new MySqlDbContextFactory(configuration),
            StaticConfiguration.SqlServer => new SqlServerDbContextFactory(configuration),
            _ => throw new Exception($"Unsupported provider: {provider}")
        });
        
        return services;
    }
}
