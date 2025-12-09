namespace Tippr.Domain.Common
{
    public interface ISoftDeleteable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt {  get; set; }
    }
}
