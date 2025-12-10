using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class Match : AuditableEntity
    {
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public Guid? TournamentGroupId { get; set; }
        public TournamentGroup? TournamentGroup { get; set; }

        public Guid HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;

        public Guid AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;

        public DateTime KickoffUtc { get; set; }
        public string Stadium { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public MatchStage Stage { get; set; }
        public MatchStatus Status { get; set; }

        // Resultat (null tills matchen är spelad)
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        // För bracket-logik (knockout)
        public Guid? ParentMatchHomeId { get; set; }
        public Guid? ParentMatchAwayId { get; set; }
    }
}
