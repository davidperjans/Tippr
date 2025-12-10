namespace Tippr.Application.Users.Models
{
    public record UserSummaryDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string? DisplayName { get; init; } = string.Empty;
    }
}
