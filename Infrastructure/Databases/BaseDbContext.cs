using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases
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
