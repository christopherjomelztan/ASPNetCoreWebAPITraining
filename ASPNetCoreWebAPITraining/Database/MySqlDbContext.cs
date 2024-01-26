using ASPNetCoreWebAPITraining.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
    public class MySqlDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options, IConfiguration configuration)
            : base(options, configuration)
        {
            _configuration = configuration;
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(8, 3, 0)));
        }
    }
}