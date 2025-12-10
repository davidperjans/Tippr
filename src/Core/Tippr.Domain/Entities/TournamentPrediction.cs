using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class TournamentPrediction : AuditableEntity
    {
        public string UserId { get; set; } = string.Empty;

        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public Guid? PredictionGroupId { get; set; }
        public PredictionGroup? PredictionGroup { get; set; }

        public TournamentPredictionType Type { get; set; }

        // För lagbaserade predictioner
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        // För spelare
        public string? PlayerName { get; set; }
        public string? PlayerCountryCode { get; set; }

        public DateTime SubmittedAtUtc { get; set; }

        public int? PointsAwarded { get; set; }
        public bool IsCorrect { get; set; }
    }
}
