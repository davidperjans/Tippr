using MediatR;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IAuthenticationService _authService;
        public LoginCommandHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(
                request.Email,
                request.Password
            );
        }
    }
}
