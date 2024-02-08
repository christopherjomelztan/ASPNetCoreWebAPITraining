using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
  public interface IDbContextFactory
  {
    BaseDbContext CreateDbContext();
  }
}