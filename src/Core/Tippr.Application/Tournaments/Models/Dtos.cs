using Tippr.Domain.Enums;

namespace Tippr.Application.Tournaments.Models
{
    public record TournamentDto(
        Guid Id,
        string Name,
        int Year,
        DateTime StartDateUtc,
        DateTime EndDateUtc
    );

    public record TournamentGroupDto(
        Guid Id,
        string Code
    );

    public record TeamDto(
        Guid Id,
        string Name,
        string FifaCode,
        string FlagUrl,
        Guid? TournamentGroupId,
        string? GroupCode
    );

    public record MatchDto(
        Guid Id,
        Guid TournamentId,
        Guid? TournamentGroupId,
        string? GroupCode,
        Guid HomeTeamId,
        string HomeTeamName,
        string HomeTeamFifaCode,
        Guid AwayTeamId,
        string AwayTeamName,
        string AwayTeamFifaCode,
        DateTime KickoffUtc,
        string Stadium,
        string City,
        MatchStage Stage,
        MatchStatus Status,
        int? HomeScore,
        int? AwayScore
    );

    public record TournamentDetailsDto(
        Guid Id,
        string Name,
        int Year,
        DateTime StartDateUtc,
        DateTime EndDateUtc,
        IReadOnlyCollection<TournamentGroupDto> Groups,
        IReadOnlyCollection<TeamDto> Teams,
        IReadOnlyCollection<MatchDto> Matches
    );
}
