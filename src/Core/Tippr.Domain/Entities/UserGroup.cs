using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class UserGroup : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public bool IsAdmin { get; set; } = false;
        public int TotalPoints { get; set; } = 0; // Cachad poäng
        public DateTime JoinedAt { get; set; }
    }
}
