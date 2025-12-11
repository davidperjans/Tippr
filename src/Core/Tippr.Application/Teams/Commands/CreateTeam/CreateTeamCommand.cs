using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Teams.Models;

namespace Tippr.Application.Teams.Commands.CreateTeam
{
    public sealed record CreateTeamCommand(
        Guid TournamentId,
        Guid TournamentGroupId,
        string Name,
        string FifaCode,
        string FlagUrl
    ) : IRequest<Result<TeamDto>>;
}
