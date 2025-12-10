using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;
using Tippr.Application.Groups.Commands.CreateGroup;
using Tippr.Application.Groups.Queries.GetGroupsByTournamentId;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GroupDto>>> CreateGroup([FromBody] CreateGroupCommand command)
        {
            var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("invalid token", new[] { "invalid token" }));
            }

            var secureCommand = command with { UserId = userId };

            var result = await _mediator.Send(secureCommand);

            return StatusCode(201, result);
        }

        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<ApiResponse<GroupDto>>> GetGroupsByTournamentId([FromBody] GetGroupsByTournamentIdQuery query)
        {
            var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("invalid token", new[] { "invalid token" }));
            }

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
