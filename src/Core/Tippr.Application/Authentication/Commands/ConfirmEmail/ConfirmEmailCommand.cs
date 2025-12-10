using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(
        string UserId,
        string Token
    ) : IRequest<Result>;
}
