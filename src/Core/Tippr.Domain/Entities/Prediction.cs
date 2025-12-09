using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class Prediction : BaseEntity, IAuditableEntity
    {
        public string UserId { get; set; } = string.Empty;

        public int MatchId { get; set; }
        public Match? Match { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public int PointsEarned { get; set; } = 0;
        public bool IsLocked { get; set; } = false; // Låses vid matchstart

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
