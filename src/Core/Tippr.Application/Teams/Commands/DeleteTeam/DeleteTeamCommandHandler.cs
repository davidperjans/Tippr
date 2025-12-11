using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Commands.DeleteTeam
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, Result>
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTeamCommandHandler(IRepository<Team> teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.Id, cancellationToken);

            if (team == null)
                return Result.Failure("Team not found.");

            _teamRepository.Remove(team);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
