using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.FifaCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.HasIndex(t => new { t.TournamentId, t.FifaCode })
                .IsUnique();

            builder.HasOne(t => t.TournamentGroup)
                .WithMany(g => g.Teams)
                .HasForeignKey(t => t.TournamentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
