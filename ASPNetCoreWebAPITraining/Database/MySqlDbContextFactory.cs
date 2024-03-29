using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public class MySqlDbContextFactory : IDbContextFactory
  {
    private readonly IConfiguration _configuration;

    public MySqlDbContextFactory(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public BaseDbContext CreateDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
      optionsBuilder.UseMySql(_configuration.GetConnectionString(StaticConfiguration.MySql), new MySqlServerVersion(StaticConfiguration.MySqlVersion));
      return new MySqlDbContext(optionsBuilder.Options, _configuration);
    }
  }
}