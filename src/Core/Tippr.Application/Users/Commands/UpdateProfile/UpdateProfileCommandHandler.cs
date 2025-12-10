using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;
using Tippr.Application.Interfaces.Services;

namespace Tippr.Application.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApiResponse<UserDto>>
    {
        private readonly IUserService _userService;
        public UpdateProfileCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApiResponse<UserDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.UpdateUserProfileAsync(
                request.UserId,
                request.FirstName,
                request.LastName,
                request.ProfilePictureUrl
            );

            return ApiResponse<UserDto>.SuccessResponse(userDto, "Profile updated successfully");
        }
    }
}
