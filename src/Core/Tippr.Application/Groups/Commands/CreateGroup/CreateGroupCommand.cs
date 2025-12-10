using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;

namespace Tippr.Application.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand(
        string Name,
        string Description,
        int TournamentId,
        int? MaxMembers
    ) : IRequest<ApiResponse<GroupDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
