using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.JoinCode).IsRequired().HasMaxLength(10);
            builder.HasIndex(g => g.JoinCode).IsUnique();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(g => g.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.Tournament)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
