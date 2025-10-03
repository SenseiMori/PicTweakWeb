using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Infrastructure.Entities.Configurations
{
    public class SessionLogConfiguration : IEntityTypeConfiguration<SessionLog>
    {
        public void Configure(EntityTypeBuilder<SessionLog> builder)
        {
            builder.HasKey(x => x.SessionId);
            builder.Property(x => x.SpentTime);
            builder.Property(x => x.IsAuthorized);
            builder.Property(x => x.SessionCreatedAt);
            builder.HasMany(x => x.ImageLogs)
                    .WithOne(p => p.Session)
                    .HasForeignKey(p => p.SessionId);
            builder.HasOne(x => x.UserLog).WithMany(x => x.SessionLogs).HasForeignKey(x => x.UserLogId);

            builder.OwnsOne(x => x.Options, options =>
                {
                    options.Property(p => p.SizeScale).HasColumnName("SizeScale");
                    options.Property(p => p.Compress).HasColumnName("Compress");
                    options.Property(p => p.RemoveEXIF).HasColumnName("RemoveEXIF");
                });
        }
    }
}
