using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;

namespace Tippr.Application.Tournaments.Commands.DeleteTournament
{
    public sealed class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, Result>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTournamentCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork)
        {
            _tournamentRepository = tournamentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _tournamentRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return Result.Failure("Tournament not found.");

            _tournamentRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
