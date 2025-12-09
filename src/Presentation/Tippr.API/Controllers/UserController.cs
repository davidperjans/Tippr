using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tippr.Application.Authentication.Queries.GetUserProfile;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;
using Tippr.Application.Users.Commands.ChangePassword;
using Tippr.Application.Users.Commands.UpdateProfile;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetMe()
        {
            var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("invalid token", new[] { "invalid token" }));
            }

            var userDto = await _mediator.Send(new GetUserProfileQuery(userId));

            if (userDto == null)
                return NotFound(ApiResponse<string>.FailureResponse("user not found", new[] { "User not found" }));

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto));
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("invalid token", new[] { "invalid token" }));
            }

            var secureCommand = command with { UserId = userId };

            var result = await _mediator.Send(secureCommand);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("invalid token", new[] { "invalid token" }));
            }

            var secureCommand = command with { UserId = userId };

            var result = await _mediator.Send(secureCommand);

            return Ok(result);
        }
    }
}
