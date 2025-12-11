using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class PredictionGroup : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool IsGlobalOfficial { get; set; } = false;
        public string JoinCode { get; set; } = string.Empty; // typ 6-tecken kod
        public string CreatedByUserId { get; set; } = string.Empty;

        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public PredictionGroupSettings Settings { get; set; } = null!;

        public ICollection<PredictionGroupMember> Members { get; set; } = new List<PredictionGroupMember>();
    }
}
