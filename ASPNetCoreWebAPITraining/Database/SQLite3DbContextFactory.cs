using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public class SQLite3DbContextFactory : IDbContextFactory
  {
    private readonly IConfiguration _configuration;

    public SQLite3DbContextFactory(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public BaseDbContext CreateDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<SQLite3DbContext>();
      optionsBuilder.UseSqlite(_configuration.GetConnectionString(StaticConfiguration.SQLite3));
      return new SQLite3DbContext(optionsBuilder.Options, _configuration);
    }
  }
}