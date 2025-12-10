using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.PredictionGroups.Commands.LeavePredictionGroup
{
    public record LeavePredictionGroupCommand(
        Guid PredictionGroupId
    ) : IRequest<Result>;
}
