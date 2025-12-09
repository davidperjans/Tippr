using Microsoft.AspNetCore.Identity;
using Tippr.Domain.Common;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt {  get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}
