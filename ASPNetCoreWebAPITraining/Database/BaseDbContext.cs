using ASPNetCoreWebAPITraining.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public class BaseDbContext : DbContext
  {
    public BaseDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Person>().HasData(SeedData.Persons);
    }
  }
}
