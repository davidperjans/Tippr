using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;

namespace Tippr.Application.Teams.Commands.DeleteTeam
{
    public sealed record DeleteTeamCommand(Guid Id) : IRequest<Result>;
}
