using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tippr.Domain.Common;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<TournamentGroup> TournamentGroups => Set<TournamentGroup>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Match> Matches => Set<Match>();

        public DbSet<PredictionGroup> PredictionGroups => Set<PredictionGroup>();
        public DbSet<PredictionGroupMember> PredictionGroupMembers => Set<PredictionGroupMember>();
        public DbSet<PredictionGroupSettings> PredictionGroupSettings => Set<PredictionGroupSettings>();
        public DbSet<ScoringConfig> ScoringConfigs => Set<ScoringConfig>();

        public DbSet<MatchPrediction> MatchPredictions => Set<MatchPrediction>();
        public DbSet<TournamentPrediction> TournamentPredictions => Set<TournamentPrediction>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // AuditableEntity-convention
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                        .Property<DateTime>("CreatedAtUtc")
                        .IsRequired();

                    builder.Entity(entityType.ClrType)
                        .Property<string>("CreatedBy")
                        .HasMaxLength(450);

                    builder.Entity(entityType.ClrType)
                        .Property<DateTime?>("ModifiedAtUtc");

                    builder.Entity(entityType.ClrType)
                        .Property<string?>("ModifiedBy")
                        .HasMaxLength(450);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAtUtc = utcNow;
                        // CreatedBy kan sättas via en "current user service" i Application-lagret
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedAtUtc = utcNow;
                        break;
                }
            }
        }
    }
}
