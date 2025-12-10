using System.Text.RegularExpressions;
using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class Tournament : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }

        public ICollection<TournamentGroup> Groups { get; set; } = new List<TournamentGroup>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
