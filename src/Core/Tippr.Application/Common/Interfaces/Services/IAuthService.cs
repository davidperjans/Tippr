using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(
        string email,
        string userName,
        string password,
        string? displayName,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken);

        Task<Result<AuthResponseDto>> LoginAsync(
            string email,
            string password,
            CancellationToken cancellationToken);

        Task<Result<AuthResponseDto>> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken);

        Task<Result> LogoutAsync(
            string userId,
            string? refreshToken,
            CancellationToken cancellationToken);

        Task<Result> ForgotPasswordAsync(
            string email,
            CancellationToken cancellationToken);

        Task<Result> ResetPasswordAsync(
            string userId,
            string resetToken,
            string newPassword,
            CancellationToken cancellationToken);

        Task<Result> ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword,
            CancellationToken cancellationToken);

        Task<Result> ConfirmEmailAsync(
            string userId,
            string confirmationToken,
            CancellationToken cancellationToken);
    }
}
