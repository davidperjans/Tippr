using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Teams.Commands.CreateTeam;
using Tippr.Application.Teams.Commands.DeleteTeam;
using Tippr.Application.Teams.Commands.UpdateTeam;
using Tippr.Application.Teams.Models;
using Tippr.Application.Teams.Queries.GetTeamByTournamentId;
using Tippr.Application.Tournaments.Commands.CreateTournament;
using Tippr.Application.Tournaments.Commands.DeleteTournament;
using Tippr.Application.Tournaments.Commands.UpdateTournament;
using Tippr.Application.Tournaments.Models;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/Teams")]
    [Authorize(Roles = "Admin")]
    public class AdminTeamsController : ControllerBase
    {
        private readonly ISender _mediator;
        public AdminTeamsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<TeamDto>>> Create([FromBody] CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id, [FromBody] UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Route id and command id do not match.");

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteTeamCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("by-tournament/{tournamentId:guid}")]
        public async Task<ActionResult<Result<List<TeamDto>>>> GetAllByTournamentId(Guid tournamentId, CancellationToken cancellationToken)
        {
            var query = new GetTeamsByTournamentIdQuery(tournamentId);
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        private ActionResult<Result> ToActionResult(Result result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
