using Microsoft.AspNetCore.Identity;
using Tippr.Domain.Common;
using Tippr.Domain.Entities;

namespace Tippr.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
