using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Queries.GetTournamentDetails
{
    public sealed record GetTournamentDetailsQuery(Guid Id) : IRequest<Result<TournamentDetailsDto>>;
}
