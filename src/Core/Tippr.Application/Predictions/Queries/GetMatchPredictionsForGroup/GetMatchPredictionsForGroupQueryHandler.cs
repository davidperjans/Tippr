using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.Predictions.Models;
using Tippr.Application.Predictions.Queries.GetMatchPredictionByMatch;
using Tippr.Domain.Entities;

namespace Tippr.Application.Predictions.Queries.GetMatchPredictionsForGroup
{
    public class GetMatchPredictionsForGroupQueryHandler : IRequestHandler<GetMatchPredictionsForGroupQuery, Result<IReadOnlyList<MatchPredictionDto>>>
    {
        private readonly IRepository<MatchPrediction> _predictionRepository;
        private readonly IRepository<PredictionGroupMember> _groupMemberRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public GetMatchPredictionsForGroupQueryHandler(
            IRepository<MatchPrediction> predictionRepository,
            IRepository<PredictionGroupMember> groupMemberRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _predictionRepository = predictionRepository;
            _groupMemberRepository = groupMemberRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<Result<IReadOnlyList<MatchPredictionDto>>> Handle(GetMatchPredictionsForGroupQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Result<IReadOnlyList<MatchPredictionDto>>.Failure("User not authenticated.");

            var membership = await _groupMemberRepository.ListAsync(
                m => m.PredictionGroupId == request.PredictionGroupId && m.UserId == userId,
                cancellationToken);

            if (!membership.Any())
                return Result<IReadOnlyList<MatchPredictionDto>>.Failure("User is not member of group.");

            var predictions = await _predictionRepository.ListAsync(
                x => x.PredictionGroupId == request.PredictionGroupId && x.UserId == userId,
                cancellationToken);

            var dtos = _mapper.Map<IReadOnlyList<MatchPredictionDto>>(predictions);

            return Result<IReadOnlyList<MatchPredictionDto>>.Success(dtos);
        }
    }
}
