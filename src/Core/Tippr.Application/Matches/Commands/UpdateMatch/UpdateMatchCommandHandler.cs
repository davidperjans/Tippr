using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Matches.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Commands.UpdateMatch
{
    public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result<Guid>>
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMatchCommandHandler(IRepository<Match> matchRepository, IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (match == null)
                return Result<Guid>.Failure("Match not found.");

            match.KickoffUtc = request.KickOffUtc;
            match.Stadium = request.Stadium;
            match.City = request.City;
            match.Stage = request.Stage;
            match.TournamentGroupId = request.TournamentGroupId;
            match.HomeTeamId = request.HomeTeamId;
            match.AwayTeamId = request.AwayTeamId;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(match.Id);
        }
    }
}
