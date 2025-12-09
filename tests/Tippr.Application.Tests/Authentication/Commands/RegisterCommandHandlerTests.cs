using FluentAssertions;
using Moq;
using Tippr.Application.Authentication;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;

namespace Tippr.Application.Tests.Authentication.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _authServiceMock = new Mock<IAuthenticationService>();
            _handler = new RegisterCommandHandler(_authServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenRegistrationSucceeds()
        {
            // Arrange
            var command = new RegisterCommand("new@user.com", "Password123!", "New", "User");

            var expectedResult = new AuthenticationResult
            {
                Success = true,
                Token = "jwt-token",
                RefreshToken = "refresh-token"
            };

            // Vi säger åt Mocken: "Om RegisterAsync anropas med dessa parametrar, svara med success"
            _authServiceMock.Setup(x => x.RegisterAsync(
                command.Email, command.Password, command.FirstName, command.LastName))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Token.Should().Be("jwt-token");

            // Verifiera att servicen faktiskt anropades
            _authServiceMock.Verify(x => x.RegisterAsync(
                command.Email, command.Password, command.FirstName, command.LastName), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenServiceReturnsError()
        {
            // Arrange
            var command = new RegisterCommand("existing@user.com", "Password123!", "Exist", "User");

            var expectedResult = new AuthenticationResult
            {
                Success = false,
                Errors = new[] { "Email already taken" }
            };

            _authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Email already taken");
        }
    }
}
