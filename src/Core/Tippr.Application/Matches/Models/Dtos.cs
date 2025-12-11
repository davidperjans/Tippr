using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Models
{
    public record MatchDto
    {
        public Guid Id { get; init; }

        public Guid TournamentId { get; init; }
        public Guid? TournamentGroupId { get; init; }

        public Guid HomeTeamId { get; init; }
        public string HomeTeamName { get; init; } = string.Empty;
        public string HomeTeamFifaCode { get; init; } = string.Empty;

        public Guid AwayTeamId { get; init; }
        public string AwayTeamName { get; init; } = string.Empty;
        public string AwayTeamFifaCode { get; init; } = string.Empty;

        public DateTime KickoffUtc { get; init; }
        public string Stadium { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;

        public MatchStage Stage { get; init; }
        public MatchStatus Status { get; init; }

        public int? HomeScore { get; init; }
        public int? AwayScore { get; init; }
    }
}
