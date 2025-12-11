using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Matches.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Matches.Commands.CreateMatch
{
    public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<MatchDto>>
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateMatchCommandHandler(IRepository<Match> matchRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<MatchDto>> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            var newMatch = new Match
            {
                Id = Guid.NewGuid(),
                TournamentId = request.TournamentId,
                TournamentGroupId = request.TournamentGroupId,
                HomeTeamId = request.HomeTeamId,
                AwayTeamId = request.AwayTeamId,
                KickoffUtc = request.KickoffUtc,
                Stadium = request.Stadium,
                City = request.City,
                Stage = request.Stage,
                Status = MatchStatus.NotStarted,
            };

            await _matchRepository.AddAsync(newMatch);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<MatchDto>(newMatch);
            return Result<MatchDto>.Success(dto);
        }
    }
}
