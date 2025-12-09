using System.Text.RegularExpressions;
using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class Tournament : BaseEntity, IAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
        public string? HostCountry { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relationer
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
