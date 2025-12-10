using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Tournaments.Commands.DeleteTournament
{
    public sealed record DeleteTournamentCommand(Guid Id) : IRequest<Result>;
}
