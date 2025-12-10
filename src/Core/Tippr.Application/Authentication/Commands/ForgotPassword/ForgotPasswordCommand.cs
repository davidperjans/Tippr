using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(
        string Email
    ) : IRequest<Result>;
}
