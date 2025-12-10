using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class PredictionGroupMemberConfiguration : IEntityTypeConfiguration<PredictionGroupMember>
    {
        public void Configure(EntityTypeBuilder<PredictionGroupMember> builder)
        {
            builder.ToTable("PredictionGroupMembers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(m => m.Role)
                .HasConversion<int>();

            builder.HasIndex(m => new { m.PredictionGroupId, m.UserId })
                .IsUnique();

            builder.HasOne(m => m.PredictionGroup)
                .WithMany(g => g.Members)
                .HasForeignKey(m => m.PredictionGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
