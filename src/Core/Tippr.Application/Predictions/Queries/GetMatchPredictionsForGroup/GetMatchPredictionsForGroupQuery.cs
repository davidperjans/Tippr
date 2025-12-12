using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Predictions.Models;

namespace Tippr.Application.Predictions.Queries.GetMatchPredictionsForGroup
{
    public sealed record GetMatchPredictionsForGroupQuery(
        Guid PredictionGroupId
    ) : IRequest<Result<IReadOnlyList<MatchPredictionDto>>>;
}
