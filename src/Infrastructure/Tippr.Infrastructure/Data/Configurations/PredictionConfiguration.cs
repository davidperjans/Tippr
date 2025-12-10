using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
    {
        public void Configure(EntityTypeBuilder<Prediction> builder)
        {
            // Det var denna du hade i OnModelCreating
            builder.HasOne(p => p.Match)
                .WithMany()
                .HasForeignKey(p => p.MatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
