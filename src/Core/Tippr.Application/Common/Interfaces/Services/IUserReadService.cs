using Tippr.Application.Users.Models;

namespace Tippr.Application.Common.Interfaces.Services
{
    public interface IUserReadService
    {
        Task<IReadOnlyDictionary<string, UserSummaryDto>> GetUserSummariesAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
    }
}
