using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.Logout
{
    public record LogoutCommand(
        string? RefreshToken // null → revoke all for this user
    ) : IRequest<Result>;
}
