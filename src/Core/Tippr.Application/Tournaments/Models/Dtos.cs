using Tippr.Application.Matches.Models;
using Tippr.Application.Teams.Models;
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
