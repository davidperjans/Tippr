using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.PredictionGroups.Commands.JoinPredictionGroup
{
    public record JoinPredictionGroupByCodeCommand(
        string JoinCode
    ) : IRequest<Result>;
}
