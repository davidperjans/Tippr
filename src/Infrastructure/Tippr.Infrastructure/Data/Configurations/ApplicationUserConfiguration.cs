using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.DisplayName)
                .HasMaxLength(200);

            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            builder.Property(u => u.UserName)
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .HasMaxLength(256);

            builder.HasIndex(u => u.NormalizedUserName).IsUnique();
            builder.HasIndex(u => u.NormalizedEmail);
        }
    }
}
