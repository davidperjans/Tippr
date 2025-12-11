using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Teams.Models;

namespace Tippr.Application.Teams.Commands.UpdateTeam
{
    public sealed record UpdateTeamCommand(
        Guid Id,
        Guid TournamentGroupId,
        string Name,
        string FifaCode,
        string FlagUrl
    ) : IRequest<Result<Guid>>;
}
