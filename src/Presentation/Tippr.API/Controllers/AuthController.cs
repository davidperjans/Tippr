using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Authentication.Commands.ChangePassword;
using Tippr.Application.Authentication.Commands.ConfirmEmail;
using Tippr.Application.Authentication.Commands.ForgotPassword;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Commands.Logout;
using Tippr.Application.Authentication.Commands.RefreshToken;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Commands.ResetPassword;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;
        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<AuthResponseDto>>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<AuthResponseDto>>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<AuthResponseDto>>> RefreshToken(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<Result>> Logout(
            [FromBody] LogoutCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> ForgotPassword(
            [FromBody] ForgotPasswordCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> ResetPassword(
            [FromBody] ResetPasswordCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<Result>> ChangePassword(
            [FromBody] ChangePasswordCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> ConfirmEmail(
            [FromQuery] string userId,
            [FromQuery] string token,
            CancellationToken cancellationToken)
        {
            var command = new ConfirmEmailCommand(userId, token);
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        // Hjälpmetoder – kan flyttas till BaseController
        private ActionResult<Result> ToActionResult(Result result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);

        private ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
