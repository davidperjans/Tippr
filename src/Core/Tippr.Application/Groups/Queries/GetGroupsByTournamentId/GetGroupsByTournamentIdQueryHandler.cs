using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;
using Tippr.Application.DTOs.Tournaments;
using Tippr.Application.Interfaces.Repos;
using Tippr.Domain.Entities;

namespace Tippr.Application.Groups.Queries.GetGroupsByTournamentId
{
    public class GetGroupsByTournamentIdQueryHandler : IRequestHandler<GetGroupsByTournamentIdQuery, ApiResponse<List<GroupDto>>>
    {
        private readonly IRepository<Group> _groupRepository;
        public GetGroupsByTournamentIdQueryHandler(IRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<ApiResponse<List<GroupDto>>> Handle(GetGroupsByTournamentIdQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.FindAsync(g => g.TournamentId == request.TournamentId, g => g.UserGroups);

            var dtos = groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                JoinCode = g.JoinCode,
                TournamentId = g.TournamentId,
                MemberCount = g.UserGroups != null ? g.UserGroups.Count : 0
            }).ToList();

            return ApiResponse<List<GroupDto>>.SuccessResponse(dtos);
        }
    }
}
