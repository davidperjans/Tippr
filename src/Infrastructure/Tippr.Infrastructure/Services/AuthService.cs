using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Domain.Entities;
using Tippr.Infrastructure.Auth;
using Tippr.Infrastructure.Data;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string confirmationToken, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                // Av säkerhetsskäl: returnera success ändå
                return Result.Success();
            }

            // Här kan du generera token + skicka mail senare
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: skicka e-post med resetToken (out of scope för nu)
            _ = resetToken; // tysta warning tills du använder det

            return Result.Success();
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<AuthResponseDto>.Failure("Invalid credentials.");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
                return Result<AuthResponseDto>.Failure("Invalid credentials.");

            var authResponse = await GenerateAuthResponseAsync(user, cancellationToken);

            return Result<AuthResponseDto>.Success(authResponse);
        }

        public async Task<Result> LogoutAsync(string userId, string? refreshToken, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var tokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken && r.UserId == userId, cancellationToken);

                if (tokenEntity is null)
                    return Result.Failure("Refresh token not found.");

                tokenEntity.RevokedAtUtc = DateTime.UtcNow;
                tokenEntity.RevokedReason = "User logout";

                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }

            // Revoke ALL tokens for user
            var tokens = await _dbContext.RefreshTokens
                .Where(r => r.UserId == userId && r.RevokedAtUtc == null && r.ExpiresAtUtc >= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var t in tokens)
            {
                t.RevokedAtUtc = DateTime.UtcNow;
                t.RevokedReason = "User logout (all)";
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var tokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken, cancellationToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
                return Result<AuthResponseDto>.Failure("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(tokenEntity.UserId);
            if (user == null)
                return Result<AuthResponseDto>.Failure("User not found.");

            // Token-rotation: markera gamla som revoked och skapa en ny
            tokenEntity.RevokedAtUtc = DateTime.UtcNow;
            tokenEntity.RevokedReason = "Rotated";
            var newRefreshToken = CreateRefreshToken(user.Id);
            tokenEntity.ReplacedByToken = newRefreshToken.Token;

            await _dbContext.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var accessToken = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var authUserDto = MapToAuthUserDto(user);

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAtUtc = expiresAt,
                User = authUserDto
            };

            return Result<AuthResponseDto>.Success(response);
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(string email, string userName, string password, string? displayName, string? firstName, string? lastName, CancellationToken cancellationToken)
        {
            var existingByEmail = await _userManager.FindByEmailAsync(email);

            if (existingByEmail != null)
                return Result<AuthResponseDto>.Failure("email is already registered.");

            var existingByUserName = await _userManager.FindByNameAsync(userName);

            if (existingByUserName != null)
                return Result<AuthResponseDto>.Failure("username is already taken.");


            var newUser = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                DisplayName = displayName ?? userName,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = false
            };

            var createResult = await _userManager.CreateAsync(newUser, password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return Result<AuthResponseDto>.Failure(errors);
            }

            // Create tokens
            var authResponse = await GenerateAuthResponseAsync(newUser, cancellationToken);

            return Result<AuthResponseDto>.Success(authResponse);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string resetToken, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Result.Failure(errors);
            }

            return Result.Success();
        }

        // ========================================================================================================================================
        //                                                      HELPERS
        // ========================================================================================================================================

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var accessToken = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var refreshToken = CreateRefreshToken(user.Id);
            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var userDto = MapToAuthUserDto(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAtUtc = expiresAt,
                User = userDto
            };
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken CreateRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty);

            var now = DateTime.UtcNow;

            return new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
        }

        private static AuthUserDto MapToAuthUserDto(ApplicationUser user)
            => new AuthUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = user.ProfileImageUrl
            };
    }
}
