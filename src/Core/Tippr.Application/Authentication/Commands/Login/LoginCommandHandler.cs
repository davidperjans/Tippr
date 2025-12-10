using MediatR;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(
                request.Email,
                request.Password,
                cancellationToken);
        }
    }
}
