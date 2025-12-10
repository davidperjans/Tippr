using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IAuthService _authService;

        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.ForgotPasswordAsync(
                request.Email,
                cancellationToken);
        }
    }
}
