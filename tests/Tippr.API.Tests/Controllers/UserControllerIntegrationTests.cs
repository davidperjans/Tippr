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
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;
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
    }
}
