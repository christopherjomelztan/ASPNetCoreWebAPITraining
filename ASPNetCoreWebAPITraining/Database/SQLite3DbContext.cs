using ASPNetCoreWebAPITraining.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
    public class SQLite3DbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;
        public SQLite3DbContext(DbContextOptions<SQLite3DbContext> options, IConfiguration configuration)
            : base(options, configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString(StaticConfiguration.SQLite3));
        }
    }
}