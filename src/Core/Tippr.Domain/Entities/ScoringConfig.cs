using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class ScoringConfig : AuditableEntity
    {
        public Guid PredictionGroupSettingsId { get; set; }
        public PredictionGroupSettings PredictionGroupSettings { get; set; } = null!;

        // Match-poäng
        public int ExactScorePoints { get; set; } = 5;
        public int OutcomeAndGoalDiffPoints { get; set; } = 4;
        public int OutcomeOnlyPoints { get; set; } = 3;

        // Bonusar (turneringsprediktioner)
        public int WinnerBonusPoints { get; set; } = 20;
        public int RunnerUpBonusPoints { get; set; } = 15;
        public int ThirdPlaceBonusPoints { get; set; } = 10;
        public int MvpBonusPoints { get; set; } = 10;
        public int TopScorerBonusPoints { get; set; } = 10;
    }
}
