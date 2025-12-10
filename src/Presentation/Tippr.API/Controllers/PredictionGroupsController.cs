using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Groups.Commands.CreateGroup;
using Tippr.Application.PredictionGroups.Commands.JoinPredictionGroup;
using Tippr.Application.PredictionGroups.Commands.LeavePredictionGroup;
using Tippr.Application.PredictionGroups.Models;
using Tippr.Application.PredictionGroups.Queries.GetMyPredictionGroups;
using Tippr.Application.PredictionGroups.Queries.GetPredictionGroupDetails;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PredictionGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PredictionGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<PredictionGroupDetailsDto>>> Create(
        [FromBody] CreatePredictionGroupCommand command,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("join")]
        public async Task<ActionResult<Result>> Join(
            [FromBody] JoinPredictionGroupByCodeCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("{id:guid}/leave")]
        public async Task<ActionResult<Result>> Leave(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new LeavePredictionGroupCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<Result<IReadOnlyCollection<PredictionGroupDto>>>> GetMyGroups(
            CancellationToken cancellationToken)
        {
            var query = new GetMyPredictionGroupsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<PredictionGroupDetailsDto>>> GetDetails(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetPredictionGroupDetailsQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }

        private ActionResult<Result> ToActionResult(Result result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
