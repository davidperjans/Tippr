using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class Group : BaseEntity, IAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty; // Unik kod för att gå med
        public int? MaxMembers { get; set; }

        public string CreatedById { get; set; } = string.Empty; // User ID (String från Identity)

        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}
