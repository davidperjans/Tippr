using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tippr.Infrastructure.Services;

namespace Tippr.API.Tests
{
    public class CurrentUserServiceTests
    {
        [Fact]
        public void UserId_Returns_Claim_NameIdentifier()
        {
            // Arrange
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user-123"),
                new Claim(ClaimTypes.Email, "test@example.com")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            var accessor = new HttpContextAccessor
            {
                HttpContext = httpContext
            };

            var service = new CurrentUserService(accessor);

            // Act
            var userId = service.UserId;

            // Assert
            Assert.Equal("user-123", userId);
        }
    }
}
