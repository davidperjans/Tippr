using Tippr.Domain.Enums;

namespace Tippr.Application.Predictions.Models
{
    public record MatchPredictionDto
    {
        public Guid Id { get; init; }
        public Guid MatchId { get; init; }
        public Guid? PredictionGroupId { get; init; }
        public int PredictedHomeScore { get; init; }
        public int PredictedAwayScore { get; init; }
        public DateTime SubmittedAtUtc { get; init; }
        public int? PointsAwarded { get; init; }
        public PredictionResultStatus Status { get; init; }
    }

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
