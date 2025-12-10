using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.ResetPassword
{
    public record ResetPasswordCommand(
        string UserId,
        string Token,
        string NewPassword,
        string ConfirmPassword
    ) : IRequest<Result>;
}
