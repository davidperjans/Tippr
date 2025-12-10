using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class ScoringConfigConfiguration : IEntityTypeConfiguration<ScoringConfig>
    {
        public void Configure(EntityTypeBuilder<ScoringConfig> builder)
        {
            builder.ToTable("ScoringConfigs");

            builder.HasKey(c => c.Id);
        }
    }
}
