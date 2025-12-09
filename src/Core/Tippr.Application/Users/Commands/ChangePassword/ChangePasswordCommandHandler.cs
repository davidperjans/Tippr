using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.Interfaces;

namespace Tippr.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResponse<string>>
    {
        private readonly IUserService _userService;
        public ChangePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ApiResponse<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _userService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
            return ApiResponse<string>.SuccessResponse("Password changed successfully");
        }
    }
}
