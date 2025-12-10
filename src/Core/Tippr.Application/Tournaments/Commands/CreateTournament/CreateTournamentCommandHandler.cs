using MediatR;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Tournaments;
using Tippr.Application.Interfaces.Repos;
using Tippr.Domain.Entities;

namespace Tippr.Application.Tournaments.Commands.CreateTournament
{
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, ApiResponse<TournamentDto>>
    {
        private readonly IRepository<Tournament> _tournamentRepository;
        public CreateTournamentCommandHandler(IRepository<Tournament> tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<ApiResponse<TournamentDto>> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            var newTournament = new Tournament
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                HostCountry = request.HostCountry,
                CreatedAt = DateTime.UtcNow
            };

            _tournamentRepository.Add(newTournament);
            await _tournamentRepository.SaveChangesAsync(cancellationToken);

            var dto = new TournamentDto
            {
                Id = newTournament.Id,
                Name = newTournament.Name,
                StartDate = newTournament.StartDate,
                EndDate = newTournament.EndDate,
                Status = newTournament.Status,
                HostCountry = newTournament.HostCountry
            };

            return ApiResponse<TournamentDto>.SuccessResponse(dto);
        }
    }
}
