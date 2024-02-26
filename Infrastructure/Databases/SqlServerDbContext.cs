using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases
{
    public class SqlServerDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;

        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options,
            IConfiguration configuration)
                : base(options, configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasColumnName("LastName")
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration
                .GetConnectionString(StaticConfiguration.SqlServer));
        }
    }
}