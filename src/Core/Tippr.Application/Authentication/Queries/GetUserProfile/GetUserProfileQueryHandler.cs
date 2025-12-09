using MediatR;
using Tippr.Application.DTOs.User;
using Tippr.Application.Interfaces;

namespace Tippr.Application.Authentication.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
    {
        private readonly IUserService _userService;

        public GetUserProfileQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserProfileAsync(request.UserId);
        }
    }
}
