namespace Tippr.Application.Authentication.Common
{
    public record AuthUserDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? DisplayName { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? ProfileImageUrl {  get; init; }
    }

    public record AuthResponseDto
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime ExpiresAtUtc { get; init; }
        public AuthUserDto User { get; init; } = default!;
    }
}
