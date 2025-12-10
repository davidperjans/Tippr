using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Tournaments.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Tournaments.Commands.CreateTournament
{
    public sealed class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Result<TournamentDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTournamentCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TournamentDto>> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Year = request.Year,
                StartDateUtc = request.StartDateUtc,
                EndDateUtc = request.EndDateUtc
            };

            await _tournamentRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<TournamentDto>(entity);

            return Result<TournamentDto>.Success(dto);
        }
    }
}
