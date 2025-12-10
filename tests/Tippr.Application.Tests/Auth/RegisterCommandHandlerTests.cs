using Moq;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Services;

namespace Tippr.Application.Tests.Auth
{
    public class RegisterCommandHandlerTests
    {
        [Fact]
        public async Task Handle_When_Passwords_Not_Match_Returns_Failure()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var handler = new RegisterCommandHandler(authServiceMock.Object);

            var command = new RegisterCommand(
                Email: "test@example.com",
                UserName: "david",
                Password: "Password123!",
                ConfirmPassword: "DifferentPassword!",
                DisplayName: "David",
                FirstName: "David",
                LastName: "Perjans"
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Passwords do not match", result.Error);
            authServiceMock.Verify(
                s => s.RegisterAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_When_Valid_Calls_Service_And_Returns_Result()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var handler = new RegisterCommandHandler(authServiceMock.Object);

            var command = new RegisterCommand(
                Email: "test@example.com",
                UserName: "david",
                Password: "Password123!",
                ConfirmPassword: "Password123!",
                DisplayName: "David",
                FirstName: "David",
                LastName: "Perjans"
            );

            var userDto = new AuthUserDto
            {
                Id = "user-1",
                UserName = "david",
                Email = "test@example.com",
                DisplayName = "David",
                FirstName = "David",
                LastName = "Perjans",
                ProfileImageUrl = null
            };

            var expected = Result<AuthResponseDto>.Success(
                new AuthResponseDto { AccessToken = "access", RefreshToken = "refresh", ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60), User = userDto });

            authServiceMock
                .Setup(s => s.RegisterAsync(
                    command.Email,
                    command.UserName,
                    command.Password,
                    command.DisplayName,
                    command.FirstName,
                    command.LastName,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("user-1", result.Data!.User.Id);
            authServiceMock.Verify(
                s => s.RegisterAsync(
                    command.Email,
                    command.UserName,
                    command.Password,
                    command.DisplayName,
                    command.FirstName,
                    command.LastName,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
