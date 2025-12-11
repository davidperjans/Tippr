using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Matches.Models;
using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Commands.UpdateMatch
{
    public sealed record UpdateMatchCommand(
        Guid Id,
        DateTime KickOffUtc,
        string Stadium,
        string City,
        MatchStage Stage,
        Guid TournamentGroupId,
        Guid HomeTeamId,
        Guid AwayTeamId
    ) : IRequest<Result<Guid>>;
}
