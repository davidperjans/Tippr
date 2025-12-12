using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Models;
using Tippr.Application.Tournaments.Queries.GetTournamentDetails;
using Tippr.Application.Tournaments.Queries.GetTournaments;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TournamentsController : ControllerBase
    {
        private readonly ISender _mediator;
        public TournamentsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Result<IReadOnlyCollection<TournamentDto>>>> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetTournamentsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<TournamentDetailsDto>>> GetDetails(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTournamentDetailsQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
