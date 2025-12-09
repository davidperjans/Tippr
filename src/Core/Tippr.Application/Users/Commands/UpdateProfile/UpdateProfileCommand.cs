using MediatR;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;

namespace Tippr.Application.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand(
        string UserId,
        string FirstName,
        string LastName,
        string ProfilePictureUrl
    ) : IRequest<ApiResponse<UserDto>>;
}
