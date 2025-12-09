using MediatR;
using Tippr.Application.DTOs.User;

namespace Tippr.Application.Authentication.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
    {
        private readonly IAuthenticationService _authService;

        public GetUserProfileQueryHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<UserDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _authService.GetUserProfileAsync(request.UserId);
        }
    }
}
