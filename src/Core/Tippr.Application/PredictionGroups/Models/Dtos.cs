using Tippr.Domain.Enums;

namespace Tippr.Application.PredictionGroups.Models
{
    public record ScoringConfigDto
    {
        public int ExactScorePoints { get; init; }
        public int OutcomeAndGoalDiffPoints { get; init; }
        public int OutcomeOnlyPoints { get; init; }
        public int WinnerBonusPoints { get; init; }
        public int RunnerUpBonusPoints { get; init; }
        public int ThirdPlaceBonusPoints { get; init; }
        public int MvpBonusPoints { get; init; }
        public int TopScorerBonusPoints { get; init; }
    }

    public record PredictionGroupSettingsDto
    {
        public PredictionMode PredictionMode { get; init; }
        public PredictionDeadlineStrategy DeadlineStrategy { get; init; }
        public int DeadlineMinutesBeforeKickoff { get; init; }
        public DateTime? GlobalLockTimeUtc { get; init; }
        public ScoringConfigDto Scoring { get; init; } = default!;
    }

    public record PredictionGroupDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Guid TournamentId { get; init; }
        public string TournamentName { get; init; } = string.Empty;
        public string JoinCode { get; init; } = string.Empty;
        public int MemberCount { get; init; }
        public bool IsOwner { get; init; }
    }

    public record PredictionGroupMemberDto
    {
        public Guid Id { get; init; }
        public string UserId { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public GroupRole Role { get; init; }
    }

    public record LeaderboardEntryDto
    {
        public string UserId { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public int TotalPoints { get; init; }
        public int Position { get; init; }
    }

    public record PredictionGroupLeaderboardDto
    {
        public Guid GroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public IReadOnlyCollection<LeaderboardEntryDto> Entries { get; init; }
            = Array.Empty<LeaderboardEntryDto>();
    }

    public record PredictionGroupDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Guid TournamentId { get; init; }
        public string TournamentName { get; init; } = string.Empty;
        public string JoinCode { get; init; } = string.Empty;
        public PredictionGroupSettingsDto Settings { get; init; } = default!;
        public IReadOnlyCollection<PredictionGroupMemberDto> Members { get; init; }
            = Array.Empty<PredictionGroupMemberDto>();
        public PredictionGroupLeaderboardDto Leaderboard { get; init; } = default!;
    }
}
