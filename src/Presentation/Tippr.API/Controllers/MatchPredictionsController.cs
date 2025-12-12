using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Matches.Commands.CreateMatch;
using Tippr.Application.Matches.Commands.DeleteMatch;
using Tippr.Application.Matches.Commands.UpdateMatch;
using Tippr.Application.Matches.Commands.UpdateResult;
using Tippr.Application.Matches.Models;
using Tippr.Application.Predictions.Commands.UpsertMatchPrediction;
using Tippr.Application.Predictions.Models;
using Tippr.Application.Predictions.Queries.GetMatchPredictionByMatch;
using Tippr.Application.Predictions.Queries.GetMatchPredictionsForGroup;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class MatchPredictionsController : ControllerBase
    {
        private readonly ISender _mediator;
        public MatchPredictionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<MatchPredictionDto>>> Create([FromBody] UpsertMatchPredictionCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("by-match")]
        public async Task<ActionResult<Result<MatchPredictionDto?>>> GetMatchPredictionByMatch([FromQuery] Guid matchId, [FromQuery] Guid? predictionGroupId, CancellationToken cancellationToken)
        {
            var query = new GetMatchPredictionByMatchQuery(matchId, predictionGroupId);
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("by-group")]
        public async Task<ActionResult<Result<IReadOnlyList<MatchPredictionDto>>>> GetMatchPredictionByGroup([FromQuery] Guid predictionGroupId, CancellationToken cancellationToken)
        {
            var query = new GetMatchPredictionsForGroupQuery(predictionGroupId);
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        //private ActionResult<Result> ToActionResult(Result result)
        //    => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
