using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Infrastructure.Entities.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<BaseAccount>
    {
        public void Configure(EntityTypeBuilder<BaseAccount> builder)
        {
            builder.ToTable("Accounts", schema: "Accounts");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RegistrationDate)
                .IsRequired();

            builder.HasDiscriminator<string>("AccountType")
                .HasValue<Account>("RegisteredUser")
                .HasValue<GuestAccount>("GuestUser");

            builder.HasOne(x => x.UserLog)
                .WithOne()
                .HasForeignKey<UserLog>(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.OwnsOne(x => x.NickName, options =>
            {
                options.Property(p => p.Value)
                    .HasColumnName("NickName")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.OwnsOne(x => x.Email, options =>
            {
                options.Property(p => p.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(255)
                    .IsRequired();
            });
            builder.OwnsOne(x => x.PasswordHash, options =>
            {
                options.Property(p => p.Value)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(500)
                    .IsRequired();
            });
        }
    }
    public class GuestAccountConfiguration : IEntityTypeConfiguration<GuestAccount>
    {
        public void Configure(EntityTypeBuilder<GuestAccount> builder)
        {
            builder.Property(x => x.EntryTime)
                .IsRequired();
        }
    }
}
