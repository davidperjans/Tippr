using Tippr.Domain.Entities;

namespace Tippr.Application.Interfaces.Repos
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Group?> GetByJoinCodeAsync(string joinCode);
        Task<IEnumerable<Group>> GetUserGroupsAsync(string userId);
        Task<bool> IsUniqueJoinCodeAsync(string joinCode);
    }
}
