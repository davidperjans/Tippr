using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Stadium)
                .HasMaxLength(200);

            builder.Property(m => m.City)
                .HasMaxLength(200);

            builder.Property(m => m.Stage)
                .HasConversion<int>();

            builder.Property(m => m.Status)
                .HasConversion<int>();

            builder.HasOne(m => m.TournamentGroup)
                .WithMany()
                .HasForeignKey(m => m.TournamentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => new { m.TournamentId, m.KickoffUtc });
        }
    }
}
