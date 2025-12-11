using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class ScoringConfig : AuditableEntity
    {
        public Guid PredictionGroupSettingsId { get; set; }
        public PredictionGroupSettings PredictionGroupSettings { get; set; } = null!;

        /// <summary>
        /// Points per match prediction
        /// </summary>
        public int PointsPerCorrectTeamGoals { get; set; } = 2;
        public int PointsCorrectSign { get; set; } = 3;
        public int MaxPointsPerMatch { get; set; } = 7;

        /// <summary>
        /// Points per team correct in each stage
        /// </summary>
        public int CorrectTeamInRoundOf16Points { get; set; } = 2;
        public int CorrectTeamInQuarterFinalPoints { get; set; } = 4;
        public int CorrectTeamInSemiFinalPoints { get; set; } = 6;
        public int CorrectTeamInFinalPoints { get; set; } = 8;


        /// <summary>
        /// Bonus points
        /// </summary>
        public int PlayerOfTheTournamentBonusPoints { get; set; } = 10;
        public int WinnerBonusPoints { get; set; } = 20;
        public int TopScorerBonusPoints { get; set; } = 20;
        public int MostGoalsGroupStageBonusPoints { get; set; } = 10;
        public int MostConcededGroupStageBonusPoints { get; set; } = 10;
    }
}
