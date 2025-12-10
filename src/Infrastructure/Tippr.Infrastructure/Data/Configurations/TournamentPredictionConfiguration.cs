using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class TournamentPredictionConfiguration : IEntityTypeConfiguration<TournamentPrediction>
    {
        public void Configure(EntityTypeBuilder<TournamentPrediction> builder)
        {
            builder.ToTable("TournamentPredictions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(p => p.Type)
                .HasConversion<int>();

            builder.Property(p => p.PlayerName)
                .HasMaxLength(200);

            builder.Property(p => p.PlayerCountryCode)
                .HasMaxLength(3);

            builder.HasIndex(p => new
            {
                p.UserId,
                p.TournamentId,
                p.PredictionGroupId,
                p.Type
            }).IsUnique();

            builder.HasOne(p => p.Tournament)
                .WithMany()
                .HasForeignKey(p => p.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

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
