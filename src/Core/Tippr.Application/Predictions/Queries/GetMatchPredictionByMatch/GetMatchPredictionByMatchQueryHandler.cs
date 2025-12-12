using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.Predictions.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Predictions.Queries.GetMatchPredictionByMatch
{
    public class GetMatchPredictionByMatchQueryHandler : IRequestHandler<GetMatchPredictionByMatchQuery, Result<MatchPredictionDto?>>
    {
        private readonly IRepository<MatchPrediction> _predictionRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public GetMatchPredictionByMatchQueryHandler(IRepository<MatchPrediction> predictionRepository, ICurrentUserService currentUser, IMapper mapper)
        {
            _predictionRepository = predictionRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<Result<MatchPredictionDto?>> Handle(GetMatchPredictionByMatchQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Result<MatchPredictionDto?>.Failure("User not authenticated.");

            var list = await _predictionRepository.ListAsync(
                x =>
                    x.MatchId == request.MatchId &&
                    x.UserId == userId &&
                    x.PredictionGroupId == request.PredictionGroupId,
                cancellationToken);

            var prediction = list.FirstOrDefault();
            if (prediction == null)
                return Result<MatchPredictionDto?>.Success(null);

            var dto = _mapper.Map<MatchPredictionDto>(prediction);

            return Result<MatchPredictionDto?>.Success(dto);
        }
    }
}
