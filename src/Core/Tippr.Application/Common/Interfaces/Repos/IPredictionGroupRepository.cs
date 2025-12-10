using Tippr.Domain.Entities;

namespace Tippr.Application.Common.Interfaces.Repos
{
    public interface IPredictionGroupRepository : IRepository<PredictionGroup>
    {
        Task<PredictionGroup?> GetWithMembersAndSettingsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PredictionGroup>> GetForUserAsync(
            string userId,
            CancellationToken cancellationToken = default);

        Task<PredictionGroup?> GetByJoinCodeAsync(
            string joinCode,
            CancellationToken cancellationToken = default);
    }
}
