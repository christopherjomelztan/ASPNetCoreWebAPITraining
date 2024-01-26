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
      optionsBuilder.UseMySql(_configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(8, 3, 0)));
      return new MySqlDbContext(optionsBuilder.Options, _configuration);
    }
  }
}