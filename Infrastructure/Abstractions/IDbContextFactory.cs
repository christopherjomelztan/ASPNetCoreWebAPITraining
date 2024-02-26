using Infrastructure.Databases;

namespace Infrastructure.Abstractions
{
  public interface IDbContextFactory
  {
    BaseDbContext CreateDbContext();
  }
}