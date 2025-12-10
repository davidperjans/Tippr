using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class TournamentGroupConfiguration : IEntityTypeConfiguration<TournamentGroup>
    {
        public void Configure(EntityTypeBuilder<TournamentGroup> builder)
        {
            builder.ToTable("TournamentGroups");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Code)
                .IsRequired()
                .HasMaxLength(50); // "Group A", "A", etc.

            builder.HasIndex(g => new { g.TournamentId, g.Code })
                .IsUnique();

            builder.HasOne(g => g.Tournament)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
