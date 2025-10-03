using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Infrastructure.Entities.Configurations
{
    public class ImageLogConfiguration : IEntityTypeConfiguration<ImageLog>
    {
        public void Configure(EntityTypeBuilder<ImageLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SpentTime);
            builder.Property(x => x.Size).HasMaxLength(50);
            builder.Property(x => x.WidthHeight).HasMaxLength(50);
            builder.HasOne(x => x.Session).WithMany(p => p.ImageLogs).HasForeignKey(p => p.SessionId);
            builder.Property(x => x.HasMetadata).IsRequired();
        }
    }
}
