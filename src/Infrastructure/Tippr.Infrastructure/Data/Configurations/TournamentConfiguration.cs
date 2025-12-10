using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Relationer (Valfritt att vara explicit här, men bra för tydlighet)
            builder.HasMany(t => t.Teams)
                .WithOne(team => team.Tournament)
                .HasForeignKey(team => team.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Matches)
                .WithOne(m => m.Tournament)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade); // Raderar man turneringen ryker matcherna
        }
    }
}
