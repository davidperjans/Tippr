namespace Tippr.Domain.Common
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAtUtc { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
