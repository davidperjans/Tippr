using MediatR;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IAuthenticationService _authService;
        public RegisterCommandHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName
            );
        }
    }
}
