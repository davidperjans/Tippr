using Tippr.Domain.Common;
using Tippr.Domain.Enums;

namespace Tippr.Domain.Entities
{
    public class PredictionGroupMember : AuditableEntity
    {
        public Guid PredictionGroupId { get; set; }
        public PredictionGroup PredictionGroup { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public GroupRole Role { get; set; }
    }
}
