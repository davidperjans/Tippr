using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Commands.RefreshToken;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Authentication.Queries.GetUserProfile;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("register failed", result.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(result, "Registration successful"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("login failed", result.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(result, "Login successful"));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("refresh token failed", result.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(result, "Token refreshed"));
        }

        [HttpGet("me")]
        [Authorize]
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
    }
}
