using MediatR;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResult>
    {
        private readonly IAuthenticationService _authService;
        public RefreshTokenCommandHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(
                request.Token,
                request.RefreshToken
            );
        }
    }
}
