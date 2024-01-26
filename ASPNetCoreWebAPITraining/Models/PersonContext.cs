using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Models
{
    public class PersonContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public PersonContext(DbContextOptions<PersonContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
        }
    }
}