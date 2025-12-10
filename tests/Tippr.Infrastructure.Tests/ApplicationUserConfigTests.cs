using Microsoft.EntityFrameworkCore;
using Tippr.Infrastructure.Data;
using Tippr.Infrastructure.Identity;
using Xunit;

namespace Tippr.Infrastructure.Tests
{
    public class ApplicationUserConfigTests
    {
        [Fact]
        public async Task Can_Create_And_Save_User()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new ApplicationDbContext(options);

            var user = new ApplicationUser
            {
                UserName = "david",
                NormalizedUserName = "DAVID",
                Email = "david@example.com",
                NormalizedEmail = "DAVID@EXAMPLE.COM",
                DisplayName = "David Perjans",
                ProfileImageUrl = "https://example.com/david.png"
            };

            context.Users.Add(user);

            // Act
            await context.SaveChangesAsync();

            // Assert
            var loaded = await context.Users.FirstAsync();
            Assert.Equal("david", loaded.UserName);
            Assert.Equal("David Perjans", loaded.DisplayName);
        }
    }
}
