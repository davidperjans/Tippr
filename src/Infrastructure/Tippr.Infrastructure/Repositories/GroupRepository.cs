using Microsoft.EntityFrameworkCore;
using Tippr.Application.Interfaces.Repos;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Data;

namespace Tippr.Infrastructure.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Group?> GetByJoinCodeAsync(string joinCode)
        {
            return await _context.Groups
                .Include(g => g.UserGroups)
                .FirstOrDefaultAsync(g => g.JoinCode == joinCode);
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(string userId)
        {
            return await _context.Groups
                .Include(g => g.UserGroups)
                .Where(g => g.UserGroups.Any(ug => ug.UserId == userId))
                .ToListAsync();
        }

        public async Task<bool> IsUniqueJoinCodeAsync(string joinCode)
        {
            return !await _context.Groups.AnyAsync(g => g.JoinCode == joinCode);
        }
    }
}
