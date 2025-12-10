using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Commands.CreateTournament
{
    public sealed record CreateTournamentCommand(
        string Name,
        int Year,
        DateTime StartDateUtc,
        DateTime EndDateUtc
    ) : IRequest<Result<TournamentDto>>;
}
