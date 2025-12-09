using MediatR;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName
    ) : IRequest<AuthenticationResult>;
}
