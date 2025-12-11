using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;

namespace Tippr.Application.Matches.Commands.UpdateResult
{
    public sealed record UpdateResultCommand(
        Guid Id,
        int HomeScore,
        int AwayScore
    ) : IRequest<Result<Guid>>;
}
