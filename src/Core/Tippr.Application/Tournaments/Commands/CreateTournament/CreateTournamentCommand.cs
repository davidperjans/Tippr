using MediatR;
using System.Security.Cryptography.X509Certificates;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Tournaments;
using Tippr.Domain.Enums;

namespace Tippr.Application.Tournaments.Commands.CreateTournament
{
    public record CreateTournamentCommand(
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        TournamentStatus Status,
        string HostCountry
    ) : IRequest<ApiResponse<TournamentDto>>;
}
