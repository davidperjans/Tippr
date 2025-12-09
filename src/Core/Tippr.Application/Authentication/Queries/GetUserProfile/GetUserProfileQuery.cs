using MediatR;
using Tippr.Application.DTOs.User;

namespace Tippr.Application.Authentication.Queries.GetUserProfile
{
    public record GetUserProfileQuery(string UserId) : IRequest<UserDto?>;
}
