namespace Tippr.Application.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
