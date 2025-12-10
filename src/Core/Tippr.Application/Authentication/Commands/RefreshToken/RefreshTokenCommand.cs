using MediatR;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string RefreshToken
    ) : IRequest<Result<AuthResponseDto>>;
}
