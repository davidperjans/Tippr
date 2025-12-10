using MediatR;
using Tippr.Application.Common;
using Tippr.Application.PredictionGroups.Models;

namespace Tippr.Application.PredictionGroups.Queries.GetMyPredictionGroups
{
    public record GetMyPredictionGroupsQuery() : IRequest<Result<IReadOnlyCollection<PredictionGroupDto>>>;
}
