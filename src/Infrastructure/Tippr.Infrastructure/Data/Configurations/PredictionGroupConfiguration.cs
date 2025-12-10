using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class PredictionGroupConfiguration : IEntityTypeConfiguration<PredictionGroup>
    {
        public void Configure(EntityTypeBuilder<PredictionGroup> builder)
        {
            builder.ToTable("PredictionGroups");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.JoinCode)
                .IsRequired()
                .HasMaxLength(16);

            builder.HasIndex(g => g.JoinCode)
                .IsUnique();

            builder.Property(g => g.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.HasOne(g => g.Tournament)
                .WithMany()
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.Settings)
                .WithOne(s => s.PredictionGroup)
                .HasForeignKey<PredictionGroupSettings>(s => s.PredictionGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 FK till ApplicationUser utan navigation i domänen
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(g => g.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
