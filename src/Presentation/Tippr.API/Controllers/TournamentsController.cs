using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Tournaments;
using Tippr.Application.Tournaments.Commands.CreateTournament;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TournamentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<TournamentDto>>> CreateTournament([FromBody] CreateTournamentCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
