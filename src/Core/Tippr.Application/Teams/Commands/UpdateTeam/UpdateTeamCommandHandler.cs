using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Teams.Models;
using Tippr.Application.Tournaments.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result<Guid>>
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTeamCommandHandler(IRepository<Team> teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.Id, cancellationToken);

            if (team == null)
                return Result<Guid>.Failure("Team not found.");

            team.TournamentGroupId = request.TournamentGroupId;
            team.Name = request.Name;
            team.FifaCode = request.FifaCode;
            team.FlagUrl = request.FlagUrl;

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return Result<Guid>.Success(team.Id);
        }
    }
}
