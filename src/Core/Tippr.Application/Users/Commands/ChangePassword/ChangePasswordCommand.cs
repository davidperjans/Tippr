using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;

namespace Tippr.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string UserId,
        string CurrentPassword,
        string NewPassword
    ) : IRequest<ApiResponse<string>>;
}
