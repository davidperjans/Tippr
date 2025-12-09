using Microsoft.AspNetCore.Mvc;
using Tippr.Application.Authentication;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Auth;

namespace Tippr.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authResult = await _authService.RegisterAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName
            );

            if (!authResult.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("register failed", authResult.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(authResult, "Registration successful"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResult = await _authService.LoginAsync(request.Email, request.Password);

            if (!authResult.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("login failed", authResult.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(authResult, "Login successful"));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResult = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResult.Success)
                return BadRequest(ApiResponse<string>.FailureResponse("refresh token failed", authResult.Errors));

            return Ok(ApiResponse<AuthenticationResult>.SuccessResponse(authResult, "Token refreshed"));
        }
    }
}
