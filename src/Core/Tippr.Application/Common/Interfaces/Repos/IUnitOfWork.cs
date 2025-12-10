namespace Tippr.Application.Common.Interfaces.Repos
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
