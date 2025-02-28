using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public class MicrosoftAccessDbContextFactory : IDbContextFactory
  {
    private readonly IConfiguration _configuration;

    public MicrosoftAccessDbContextFactory(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public BaseDbContext CreateDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<MicrosoftAccessDbContext>();
      optionsBuilder.UseJet(_configuration.GetConnectionString(StaticConfiguration.MicrosoftAccess));
      return new MicrosoftAccessDbContext(optionsBuilder.Options, _configuration);
    }
  }
}