using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class MatchPrediction : AuditableEntity
    {
        public string UserId { get; set; } = string.Empty;

        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public Guid? PredictionGroupId { get; set; }
        public PredictionGroup? PredictionGroup { get; set; }

        public int PredictedHomeScore { get; set; }
        public int PredictedAwayScore { get; set; }

        public DateTime SubmittedAtUtc { get; set; }

        // Denormaliserad poäng för performance / leaderboards
        public int? PointsAwarded { get; set; }
        public PredictionResultStatus Status { get; set; } = PredictionResultStatus.Pending;
    }
}
