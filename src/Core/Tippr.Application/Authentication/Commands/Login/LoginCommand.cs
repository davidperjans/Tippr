using MediatR;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password
    ) : IRequest<Result<AuthResponseDto>>;
}
