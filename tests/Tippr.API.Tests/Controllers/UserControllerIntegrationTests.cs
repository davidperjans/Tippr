using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Tippr.API.Tests.Common;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;
using Tippr.Application.Users.Commands.ChangePassword;
using Tippr.Application.Users.Commands.UpdateProfile;

namespace Tippr.API.Tests.Controllers
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UpdateProfile_ShouldUpdateName_WhenAuthorized()
        {
            // 1. ARRANGE: Registrera en användare för att få en giltig Token
            var registerCommand = new RegisterCommand("profiletest@demo.com", "P@ssword123!", "Old", "Name");
            var authResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", registerCommand);
            var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = authResult!.Data!.Token;

            // 2. ACT: Skicka UpdateProfileCommand med JWT-token
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var updateCommand = new UpdateProfileCommand(
                UserId: "ignored", // Controllern ska överskriva detta från token
                FirstName: "NewFirst",
                LastName: "NewLast",
                ProfilePictureUrl: ""
            );

            var response = await _client.PutAsJsonAsync("/api/v1/User/me", updateCommand);

            // 3. ASSERT: Kolla att uppdateringen lyckades
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            result!.Success.Should().BeTrue();
            result.Data!.FirstName.Should().Be("NewFirst");
            result.Data.LastName.Should().Be("NewLast");

            // Dubbelkolla genom att hämta profilen igen (GET)
            var getResponse = await _client.GetAsync("/api/v1/User/me");
            var getResult = await getResponse.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            getResult!.Data!.FirstName.Should().Be("NewFirst");
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnUnauthorized_WhenNoToken()
        {
            // 1. ACT: Försök uppdatera utan header
            var command = new UpdateProfileCommand("id", "Test", "Test", "");
            var response = await _client.PutAsJsonAsync("/api/v1/User/me", command);

            // 2. ASSERT
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnBadRequest_WhenInvalidData()
        {
            // 1. ARRANGE: Skaffa token
            var registerCommand = new RegisterCommand("invaliduptest@demo.com", "P@ssword123!", "Valid", "User");
            var authResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", registerCommand);
            var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = authResult!.Data!.Token;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. ACT: Skicka tomt namn (FluentValidation ska stoppa detta)
            var invalidCommand = new UpdateProfileCommand("id", "", "LastName", "");
            var response = await _client.PutAsJsonAsync("/api/v1/User/me", invalidCommand);

            // 3. ASSERT
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Om du vill vara extra noga kan du kolla felmeddelandet också
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("First name is required");
        }

        [Fact]
        public async Task ChangePassword_ShouldSucceed_WhenDataIsValid()
        {
            // 1. ARRANGE: Skapa användare
            var email = "changepass@demo.com";
            var oldPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            var registerCommand = new RegisterCommand(email, oldPassword, "Test", "User");
            var authResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", registerCommand);
            var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = authResult!.Data!.Token;

            // Logga in
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. ACT: Byt lösenord
            var changeCommand = new ChangePasswordCommand("ignored", oldPassword, newPassword);
            var response = await _client.PutAsJsonAsync("/api/v1/User/me/password", changeCommand);

            // 3. ASSERT: Bytet ska gå bra
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // 4. VERIFIERA: Testa att logga in med det NYA lösenordet (Det ultimata testet)
            var loginCommand = new LoginCommand(email, newPassword);
            var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ChangePassword_ShouldFail_WhenCurrentPasswordIsWrong()
        {
            // 1. ARRANGE
            var registerCommand = new RegisterCommand("wrongpass@demo.com", "CorrectPassword123!", "Test", "User");
            var authResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", registerCommand);
            var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = authResult!.Data!.Token;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. ACT: Skicka FEL nuvarande lösenord
            var changeCommand = new ChangePasswordCommand("ignored", "WrongPassword123!", "NewPassword123!");
            var response = await _client.PutAsJsonAsync("/api/v1/User/me/password", changeCommand);

            // 3. ASSERT
            // Identity returnerar ett fel, UserService kastar ValidationException -> Middleware returnerar 400
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            // Identitys standardfelmeddelande brukar vara "Incorrect password."
            content.Should().Contain("Incorrect password");
        }

        [Fact]
        public async Task ChangePassword_ShouldFail_WhenNewPasswordIsWeak()
        {
            // 1. ARRANGE
            var registerCommand = new RegisterCommand("weakpass@demo.com", "StrongPassword123!", "Test", "User");
            var authResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", registerCommand);
            var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = authResult!.Data!.Token;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. ACT: Skicka ett SVAGT nytt lösenord (FluentValidation ska fånga detta)
            var changeCommand = new ChangePasswordCommand("ignored", "StrongPassword123!", "weak");
            var response = await _client.PutAsJsonAsync("/api/v1/User/me/password", changeCommand);

            // 3. ASSERT
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Password must be at least 8 characters");
        }
    }
}
