using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword
    ) : IRequest<Result>;
}
