using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Matches.Models;
using Tippr.Application.Tournaments.Models;
using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Commands.CreateMatch
{
    public sealed record CreateMatchCommand(
         Guid TournamentId,
         Guid? TournamentGroupId,
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
         int? HomeScore,
         int? AwayScore
    ) : IRequest<Result<MatchDto>>;
}
