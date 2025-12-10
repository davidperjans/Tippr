using Tippr.Domain.Common;

namespace Tippr.Domain.Entities
{
    public class RefreshToken : AuditableEntity
    {
        public string UserId { get; set; } = string.Empty;

        // Själva token-strängen (i “riktig prod” vill du helst hasha den)
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string? CreatedByIp { get; set; }

        public DateTime? RevokedAtUtc { get; set; }
        public string? RevokedByIp { get; set; }
        public string? RevokedReason { get; set; }

        // För token-rotation – vilken token ersatte den här?
        public string? ReplacedByToken { get; set; }

        public bool IsActive => RevokedAtUtc is null && DateTime.UtcNow <= ExpiresAtUtc;
    }
}
