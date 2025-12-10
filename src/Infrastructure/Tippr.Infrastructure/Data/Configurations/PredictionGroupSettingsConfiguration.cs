using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class PredictionGroupSettingsConfiguration : IEntityTypeConfiguration<PredictionGroupSettings>
    {
        public void Configure(EntityTypeBuilder<PredictionGroupSettings> builder)
        {
            builder.ToTable("PredictionGroupSettings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.PredictionMode)
                .HasConversion<int>();

            builder.Property(s => s.DeadlineStrategy)
                .HasConversion<int>();

            builder.HasOne(s => s.ScoringConfig)
                .WithOne(c => c.PredictionGroupSettings)
                .HasForeignKey<ScoringConfig>(c => c.PredictionGroupSettingsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
