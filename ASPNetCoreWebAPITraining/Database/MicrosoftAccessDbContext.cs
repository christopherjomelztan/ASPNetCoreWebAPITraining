using System.Runtime.Versioning;
using ASPNetCoreWebAPITraining.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
    
    public class MicrosoftAccessDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;
        public MicrosoftAccessDbContext(DbContextOptions<MicrosoftAccessDbContext> options, IConfiguration configuration)
            : base(options, configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseJet(_configuration.GetConnectionString(StaticConfiguration.MicrosoftAccess));
        }
    }
}