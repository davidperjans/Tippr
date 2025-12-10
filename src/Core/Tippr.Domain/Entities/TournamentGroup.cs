using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class TournamentGroup : AuditableEntity
    {
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public string Code { get; set; } = string.Empty;  // "Group A", "Group B" osv.

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
