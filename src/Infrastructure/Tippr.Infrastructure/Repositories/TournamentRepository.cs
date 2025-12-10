using Microsoft.EntityFrameworkCore;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Data;

namespace Tippr.Infrastructure.Repositories
{
    public class TournamentRepository : Repository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Tournament>> GetAllWithBasicInfoAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Tournaments
                .AsNoTracking()
                .OrderBy(t => t.StartDateUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<Tournament?> GetDetailsAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _context.Tournaments
                .Include(t => t.Groups)
                .Include(t => t.Teams)
                    .ThenInclude(team => team.TournamentGroup)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.HomeTeam)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.AwayTeam)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}
