using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Predictions.Models;

namespace Tippr.Application.Predictions.Queries.GetMatchPredictionByMatch
{
    public sealed record GetMatchPredictionByMatchQuery(
        Guid MatchId,
        Guid? PredictionGroupId
    ) : IRequest<Result<MatchPredictionDto?>>;
}
