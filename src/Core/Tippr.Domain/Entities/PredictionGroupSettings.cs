using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class PredictionGroupSettings : AuditableEntity
    {
        public Guid PredictionGroupId { get; set; }
        public PredictionGroup PredictionGroup { get; set; } = null!;

        public PredictionMode PredictionMode { get; set; }
        public PredictionDeadlineStrategy DeadlineStrategy { get; set; }

        // T.ex. 60 minuter innan avspark
        public int DeadlineMinutesBeforeKickoff { get; set; }

        // För "AllAtOnce" eller stage-by-stage
        public DateTime? GlobalLockTimeUtc { get; set; }

        public ScoringConfig ScoringConfig { get; set; } = null!;
    }
}
