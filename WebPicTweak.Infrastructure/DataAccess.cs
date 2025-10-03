using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPicTweak.Core.Models.Image;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Infrastructure
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<SessionLog> SessionLogs { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ImageLog> ImageLogs { get; set; }
        public DbSet<BaseAccount> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Logs");
            modelBuilder.Owned<ModifierOptions>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
    public static class DataAccess
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<LogDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("LogDatabase"));
            });
            return services;
        }
    }
}
