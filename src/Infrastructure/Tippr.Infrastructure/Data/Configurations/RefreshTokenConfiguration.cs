using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.CreatedByIp)
                .HasMaxLength(100);

            builder.Property(r => r.RevokedByIp)
                .HasMaxLength(100);

            builder.Property(r => r.RevokedReason)
                .HasMaxLength(500);

            builder.Property(r => r.ReplacedByToken)
                .HasMaxLength(500);

            // FK → ApplicationUser, utan navigation i domänen
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index för att slå upp tokens snabbt
            builder.HasIndex(r => r.Token).IsUnique();
        }
    }
}
