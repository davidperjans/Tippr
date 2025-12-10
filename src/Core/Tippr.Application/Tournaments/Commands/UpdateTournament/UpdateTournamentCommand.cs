using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Commands.UpdateTournament
{
    public sealed record UpdateTournamentCommand(
        Guid Id,
        string Name,
        int Year,
        DateTime StartDateUtc,
        DateTime EndDateUtc
    ) : IRequest<Result<TournamentDto>>;
}
