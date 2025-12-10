using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return _authService.ConfirmEmailAsync(
                request.UserId,
                request.Token,
                cancellationToken);
        }
    }
}
