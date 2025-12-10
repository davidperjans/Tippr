using Tippr.Domain.Enums;

namespace Tippr.Application.Tournaments.Models
{
    public record TournamentDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Year { get; init; }
        public DateTime StartDateUtc { get; init; }
        public DateTime EndDateUtc { get; init; }
    }

    public record TournamentGroupDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
    }

    public record TeamDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string FifaCode { get; init; } = string.Empty;
        public string FlagUrl { get; init; } = string.Empty;

        public Guid? TournamentGroupId { get; init; }
        public string? GroupCode { get; init; }
    }

    public record MatchDto
    {
        public Guid Id { get; init; }

        public Guid TournamentId { get; init; }
        public Guid? TournamentGroupId { get; init; }
        public string? GroupCode { get; init; }

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

    public record TournamentDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Year { get; init; }
        public DateTime StartDateUtc { get; init; }
        public DateTime EndDateUtc { get; init; }

        public IReadOnlyCollection<TournamentGroupDto> Groups { get; init; }
            = Array.Empty<TournamentGroupDto>();

        public IReadOnlyCollection<TeamDto> Teams { get; init; }
            = Array.Empty<TeamDto>();

        public IReadOnlyCollection<MatchDto> Matches { get; init; }
            = Array.Empty<MatchDto>();
    }
}
