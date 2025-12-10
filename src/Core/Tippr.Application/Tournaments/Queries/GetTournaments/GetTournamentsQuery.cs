using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Queries.GetTournaments
{
    public sealed record GetTournamentsQuery : IRequest<Result<IReadOnlyCollection<TournamentDto>>>;
}
