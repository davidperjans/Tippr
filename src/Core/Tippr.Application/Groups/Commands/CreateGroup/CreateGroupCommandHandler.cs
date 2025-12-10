using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;
using Tippr.Application.Interfaces.Repos;
using Tippr.Domain.Entities;

namespace Tippr.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, ApiResponse<GroupDto>>
    {
        private readonly IGroupRepository _groupRepository;
        public CreateGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<ApiResponse<GroupDto>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var joinCode = await GenerateUniqueJoinCodeAsync();

            var group = new Group
            {
                Name = request.Name,
                Description = request.Description,
                TournamentId = request.TournamentId,
                MaxMembers = request.MaxMembers,
                CreatedById = request.UserId,
                JoinCode = joinCode,
                CreatedAt = DateTime.UtcNow
            };

            group.UserGroups.Add(new UserGroup
            {
                UserId = request.UserId,
                IsAdmin = true,
                JoinedAt = DateTime.UtcNow
            });

            _groupRepository.Add(group);
            await _groupRepository.SaveChangesAsync(cancellationToken);

            var dto = new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                JoinCode = group.JoinCode,
                TournamentId = group.TournamentId,
                MemberCount = 1,
                IsAdmin = true
            };

            return ApiResponse<GroupDto>.SuccessResponse(dto);
        }

        private async Task<string> GenerateUniqueJoinCodeAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            while (true)
            {
                var code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                if (await _groupRepository.IsUniqueJoinCodeAsync(code))
                    return code;
            }
        }
    }
}
