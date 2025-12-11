using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Teams.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateTeamCommandHandler(IRepository<Team> teamRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<TeamDto>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var newTeam = new Team
            {
                Id = Guid.NewGuid(),
                TournamentId = request.TournamentId,
                TournamentGroupId = request.TournamentGroupId,
                Name = request.Name,
                FifaCode = request.FifaCode,
                FlagUrl = request.FlagUrl
            };

            await _teamRepository.AddAsync(newTeam);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<TeamDto>(newTeam);
            return Result<TeamDto>.Success(dto);
        }
    }
}
