using Domain.Common;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases
{
  public class SqlServerDbContextFactory : IDbContextFactory
  {
    private readonly IConfiguration _configuration;

    public SqlServerDbContextFactory(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public BaseDbContext CreateDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
      optionsBuilder.UseSqlServer(_configuration.GetConnectionString(StaticConfiguration.SqlServer));
      return new SqlServerDbContext(optionsBuilder.Options, _configuration);
    }
  }
}