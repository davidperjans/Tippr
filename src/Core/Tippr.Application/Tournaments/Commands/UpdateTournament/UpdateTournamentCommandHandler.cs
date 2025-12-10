using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Commands.UpdateTournament
{
    public sealed class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, Result<TournamentDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTournamentCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TournamentDto>> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _tournamentRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return Result<TournamentDto>.Failure("Tournament not found.");

            entity.Name = request.Name;
            entity.Year = request.Year;
            entity.StartDateUtc = request.StartDateUtc;
            entity.EndDateUtc = request.EndDateUtc;

            _tournamentRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<TournamentDto>(entity);

            return Result<TournamentDto>.Success(dto);
        }
    }
}
