using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Domain.Entities;

namespace Tippr.Application.Matches.Commands.DeleteMatch
{
    public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result>
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteMatchCommandHandler(IRepository<Match> matchRepository, IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (match == null)
                return Result.Failure("Match not found.");

            _matchRepository.Remove(match);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
