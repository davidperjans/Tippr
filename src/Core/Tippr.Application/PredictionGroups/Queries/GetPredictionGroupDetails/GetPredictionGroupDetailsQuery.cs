using MediatR;
using Tippr.Application.Common;
using Tippr.Application.PredictionGroups.Models;

namespace Tippr.Application.PredictionGroups.Queries.GetPredictionGroupDetails
{
    public record GetPredictionGroupDetailsQuery(Guid PredictionGroupId) : IRequest<Result<PredictionGroupDetailsDto>>;
}
