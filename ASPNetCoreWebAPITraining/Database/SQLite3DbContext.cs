using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
    public class SQLite3DbContext : BaseDbContext
    {
        public SQLite3DbContext(DbContextOptions<SQLite3DbContext> options)
            : base(options)
        {
        }
    }
}
