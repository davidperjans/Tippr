using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Predictions.Models;

namespace Tippr.Application.Predictions.Commands.UpsertMatchPrediction
{
    public sealed record UpsertMatchPredictionCommand(
        Guid MatchId,
        Guid? PredictionGroupId,
        int PredictedHomeScore,
        int PredictedAwayScore
    ) : IRequest<Result<MatchPredictionDto>>;
}
