using Microsoft.EntityFrameworkCore;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.Users.Models;
using Tippr.Infrastructure.Data;

namespace Tippr.Infrastructure.Services
{
    public class UserReadService : IUserReadService
    {
        private readonly ApplicationDbContext _context;

        public UserReadService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyDictionary<string, UserSummaryDto>> GetUserSummariesAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
        {
            var ids = userIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            if (ids.Count == 0)
                return new Dictionary<string, UserSummaryDto>();

            var users = await _context.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => new UserSummaryDto
                {
                    Id = u.Id,
                    UserName = u.UserName ?? string.Empty,
                    DisplayName = u.DisplayName ?? u.UserName
                })
                .ToListAsync(cancellationToken);

            return users.ToDictionary(u => u.Id, u => u);
        }
    }
}
