using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Matches.Commands.CreateMatch;
using Tippr.Application.Matches.Commands.DeleteMatch;
using Tippr.Application.Matches.Commands.UpdateMatch;
using Tippr.Application.Matches.Commands.UpdateResult;
using Tippr.Application.Matches.Models;
using Tippr.Application.Teams.Commands.CreateTeam;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/Matches")]
    [Authorize(Roles = "Admin")]
    public class AdminMatchesController : ControllerBase
    {
        private readonly ISender _mediator;
        public AdminMatchesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<MatchDto>>> Create([FromBody] CreateMatchCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id, [FromBody] UpdateMatchCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Route id and command id do not match.");

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMatchCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id:guid}/result")]
        public async Task<ActionResult<Result<Guid>>> UpdateResult(Guid id, [FromBody] UpdateResultCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Route id and command id do not match.");

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        private ActionResult<Result> ToActionResult(Result result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
