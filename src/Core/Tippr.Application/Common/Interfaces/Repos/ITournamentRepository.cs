using Tippr.Domain.Entities;

namespace Tippr.Application.Common.Interfaces.Repos
{
    public interface ITournamentRepository : IRepository<Tournament>
    {
        Task<IReadOnlyList<Tournament>> GetAllWithBasicInfoAsync(
            CancellationToken cancellationToken);

        Task<Tournament?> GetDetailsAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
