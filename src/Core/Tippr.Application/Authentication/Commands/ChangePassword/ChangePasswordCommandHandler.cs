using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUser;

        public ChangePasswordCommandHandler(IAuthService authService, ICurrentUserService currentUser)
        {
            _authService = authService;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result.Failure("Passwords do not match.");
            }

            var userId = _currentUser.UserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result.Failure("User is not authenticated.");
            }

            return await _authService.ChangePasswordAsync(
                userId,
                request.CurrentPassword,
                request.NewPassword,
                cancellationToken);
        }
    }
}
