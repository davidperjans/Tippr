using MediatR;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;

namespace Tippr.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string UserName,
        string Password,
        string ConfirmPassword,
        string? DisplayName,
        string? FirstName,
        string? LastName
    ) : IRequest<Result<AuthResponseDto>>;
}
