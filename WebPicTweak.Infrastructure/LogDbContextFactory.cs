using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebPicTweak.Infrastructure
{
    public class LogDbContextFactory : IDesignTimeDbContextFactory<LogDbContext>
    {
        public LogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LogDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=testWebPicTweakLogs;Username=postgres;Password=admin");
            return new LogDbContext(optionsBuilder.Options);
        }
    }
}
