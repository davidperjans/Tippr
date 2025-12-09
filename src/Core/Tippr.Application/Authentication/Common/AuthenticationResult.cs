namespace Tippr.Application.Authentication.Common
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
