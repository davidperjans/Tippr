using Microsoft.EntityFrameworkCore;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Data;

namespace Tippr.Infrastructure.Repositories
{
    public class PredictionGroupRepository : Repository<PredictionGroup>, IPredictionGroupRepository
    {
        public PredictionGroupRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<PredictionGroup?> GetByJoinCodeAsync(string joinCode, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(g => g.Members)
                .Include(g => g.Settings)
                    .ThenInclude(s => s.ScoringConfig)
                .FirstOrDefaultAsync(
                    g => g.JoinCode == joinCode,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<PredictionGroup>> GetForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<PredictionGroup?> GetWithMembersAndSettingsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(g => g.Settings)
                    .ThenInclude(s => s.ScoringConfig)
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }
    }
}
