using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Commands.CreateTournament;
using Tippr.Application.Tournaments.Commands.DeleteTournament;
using Tippr.Application.Tournaments.Commands.UpdateTournament;
using Tippr.Application.Tournaments.Models;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/Tournaments")]
    [Authorize(Roles = "Admin")]
    public class AdminTournamentsController : Controller
    {
        private readonly ISender _mediator;
        public AdminTournamentsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<TournamentDto>>> Create([FromBody] CreateTournamentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<TournamentDto>>> Update(Guid id, [FromBody] UpdateTournamentCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Route id and command id do not match.");

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteTournamentCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        private ActionResult<Result> ToActionResult(Result result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
