using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class MatchPredictionConfiguration : IEntityTypeConfiguration<MatchPrediction>
    {
        public void Configure(EntityTypeBuilder<MatchPrediction> builder)
        {
            builder.ToTable("MatchPredictions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(p => p.Status)
                .HasConversion<int>();

            builder.HasIndex(p => new { p.UserId, p.MatchId, p.PredictionGroupId })
                .IsUnique();

            builder.HasOne(p => p.Match)
                .WithMany()
                .HasForeignKey(p => p.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.PredictionGroup)
                .WithMany()
                .HasForeignKey(p => p.PredictionGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
