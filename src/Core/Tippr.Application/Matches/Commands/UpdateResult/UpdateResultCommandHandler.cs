using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Commands.UpdateResult
{
    public class UpdateResultCommandHandler : IRequestHandler<UpdateResultCommand, Result<Guid>>
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateResultCommandHandler(IRepository<Match> matchRepository, IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateResultCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (match == null)
                return Result<Guid>.Failure("Match not found.");

            match.HomeScore = request.HomeScore;
            match.AwayScore = request.AwayScore;
            match.Status = MatchStatus.Finished;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(match.Id);
        }
    }
}
