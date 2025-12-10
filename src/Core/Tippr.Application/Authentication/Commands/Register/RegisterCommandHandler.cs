using MediatR;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return Result<AuthResponseDto>.Failure("Passwords do not match.");
            }

            return await _authService.RegisterAsync(
                request.Email,
                request.UserName,
                request.Password,
                request.DisplayName,
                request.FirstName,
                request.LastName,
                cancellationToken);
        }
    }
}
