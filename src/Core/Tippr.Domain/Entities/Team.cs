using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class Team : BaseEntity, IAuditableEntity
    {
        public string Name { get; set; } = string.Empty;     // T.ex. "Sweden"
        public string ShortName { get; set; } = string.Empty; // T.ex. "SWE"
        public string? FlagUrl { get; set; }
        public string? GroupLetter { get; set; } // T.ex. "A" (för gruppspelet i VM)

        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
