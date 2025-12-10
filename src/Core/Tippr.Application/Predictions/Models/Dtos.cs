using Tippr.Domain.Enums;

namespace Tippr.Application.Predictions.Models
{
    public record MatchPredictionDto(
        Guid Id,
        Guid MatchId,
        Guid? PredictionGroupId,
        string UserId,
        int PredictedHomeScore,
        int PredictedAwayScore,
        DateTime SubmittedAtUtc,
        int? PointsAwarded,
        PredictionResultStatus Status
    );

    public record TournamentPredictionDto(
        Guid Id,
        Guid TournamentId,
        Guid? PredictionGroupId,
        string UserId,
        TournamentPredictionType Type,
        Guid? TeamId,
        string? TeamName,
        string? PlayerName,
        string? PlayerCountryCode,
        DateTime SubmittedAtUtc,
        int? PointsAwarded,
        bool IsCorrect
    );
}
