using Moq;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Tests.Auth
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_Service_Result()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var handler = new LoginCommandHandler(authServiceMock.Object);

            var command = new LoginCommand("test@example.com", "password123");

            var expectedUser = new AuthUserDto
            {
                Id = "user-1",
                UserName = "david",
                Email = "test@example.com",
                DisplayName = "David",
                FirstName = "David",
                LastName = "Perjans",
                ProfileImageUrl = null
            };

            var expectedResult = Result<AuthResponseDto>.Success(
                new AuthResponseDto
                {
                    AccessToken = "access",
                    RefreshToken = "refresh",
                    ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                    User = expectedUser
                });

            authServiceMock
                .Setup(s => s.LoginAsync(command.Email, command.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("david", result.Data!.User.UserName);
            authServiceMock.Verify(
                s => s.LoginAsync(command.Email, command.Password, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
