using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.Teams.Models;

namespace Tippr.Application.Teams.Queries.GetTeamByTournamentId
{
    public sealed record GetTeamsByTournamentIdQuery(Guid Id) : IRequest<Result<List<TeamDto>>>;
}
