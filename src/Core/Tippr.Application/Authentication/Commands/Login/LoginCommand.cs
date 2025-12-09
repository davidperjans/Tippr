using MediatR;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password
    ) : IRequest<AuthenticationResult>;
}
