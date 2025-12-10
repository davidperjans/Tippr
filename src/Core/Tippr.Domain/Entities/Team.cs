using System.Text.RegularExpressions;
using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class Team : AuditableEntity
    {
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public Guid? TournamentGroupId { get; set; }
        public TournamentGroup? TournamentGroup { get; set; }

        public string Name { get; set; } = string.Empty;      // "Germany"
        public string FifaCode { get; set; } = string.Empty;  // "GER"
        public string FlagUrl { get; set; } = string.Empty;

        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    }
}
