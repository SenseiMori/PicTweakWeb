using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Infrastructure.Entities.Configurations
{
    public class UserLogConfiguration : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.HasKey(e => e.UserLogId);
            builder.Property(e => e.RegistrationDate).IsRequired();
            builder.HasMany(x => x.SessionLogs).WithOne(x => x.UserLog).HasForeignKey(x => x.UserLogId);
            builder.HasOne(x => x.AccountBase).WithOne(x => x.UserLog).HasForeignKey<UserLog>(x => x.AccountId);
        }
    }
}
