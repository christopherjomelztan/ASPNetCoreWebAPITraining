using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public class BaseDbContext : DbContext
  {
    private readonly IConfiguration _configuration;
    public BaseDbContext(DbContextOptions options, IConfiguration configuration)
        : base(options)
    {
      _configuration = configuration;
    }

    public DbSet<Person> Persons { get; set; }
  }
}
