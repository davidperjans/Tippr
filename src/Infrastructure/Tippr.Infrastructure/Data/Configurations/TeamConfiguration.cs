using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.ShortName).IsRequired().HasMaxLength(3); // "SWE"
            builder.Property(t => t.GroupLetter).HasMaxLength(1); // "A"
        }
    }
}
