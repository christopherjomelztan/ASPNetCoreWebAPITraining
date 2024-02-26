using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Databases
{
    public class MySqlDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options,
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
            optionsBuilder.UseMySql(_configuration.GetConnectionString(StaticConfiguration.MySql),
                new MySqlServerVersion(StaticConfiguration.MySqlVersion));
        }
    }
}