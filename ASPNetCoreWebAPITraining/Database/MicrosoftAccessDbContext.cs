using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Database
{
    public class MicrosoftAccessDbContext : BaseDbContext
    {
        public MicrosoftAccessDbContext(DbContextOptions<MicrosoftAccessDbContext> options)
            : base(options)
        {
        }
    }
}
