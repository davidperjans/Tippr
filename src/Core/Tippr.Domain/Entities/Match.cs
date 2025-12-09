using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class Match : BaseEntity, IAuditableEntity
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        // Kan vara null om lagen inte är bestämda än (slutspel)
        public int? HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }

        public DateTime MatchDate { get; set; }
        public string? Venue { get; set; }
        public MatchStage Stage { get; set; } // Gruppspel, Final etc.

        // Resultat
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
