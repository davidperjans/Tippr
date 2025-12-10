using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;

namespace Tippr.Application.Groups.Queries.GetGroupsByTournamentId
{
    public record GetGroupsByTournamentIdQuery(
        int TournamentId
    ) : IRequest<ApiResponse<List<GroupDto>>>;
}
