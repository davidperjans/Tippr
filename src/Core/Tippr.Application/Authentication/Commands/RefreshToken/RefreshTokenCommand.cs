using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string Token,
        string RefreshToken
    ) : IRequest<AuthenticationResult>;
}
