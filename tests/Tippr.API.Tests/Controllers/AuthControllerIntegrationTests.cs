using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tippr.API.Tests.Common;
using Tippr.Application.Authentication.Commands.Login;
using Tippr.Application.Authentication.Commands.Register;
using Tippr.Application.Authentication.Common;
using Tippr.Application.Common;
using Tippr.Application.DTOs.User;

namespace Tippr.API.Tests.Controllers
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullAuthFlow_ShouldWorkCorrectly()
        {
            // 1. REGISTER
            var registerCommand = new RegisterCommand("testintegration@demo.com", "P@ssword123!", "Integration", "Test");

            var registerResponse = await _client.PostAsJsonAsync("/api/v1/Auth/register", registerCommand);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var registerResult = await registerResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            registerResult.Success.Should().BeTrue();
            registerResult.Data.Token.Should().NotBeNullOrEmpty();

            // 2. LOGIN (Testar att vi kan logga in med det vi nyss skapade)
            var loginCommand = new LoginCommand("testintegration@demo.com", "P@ssword123!");

            var loginResponse = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginCommand);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<AuthenticationResult>>();
            var token = loginResult.Data.Token;

            // 3. GET PROFILE (Protected Route)
            // Först utan token -> Ska ge 401
            var unauthorizedResponse = await _client.GetAsync("/api/v1/Auth/me");
            unauthorizedResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Sen med token -> Ska ge 200 OK
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var meResponse = await _client.GetAsync("/api/v1/Auth/me");

            meResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var meResult = await meResponse.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            meResult.Data.Email.Should().Be("testintegration@demo.com");

            if (registerResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorContent = await registerResponse.Content.ReadAsStringAsync();
                throw new Exception($"API Error Content: {errorContent}");
            }
        }

        [Fact]
        public async Task Register_WithInvalidEmail_ShouldReturnBadRequest()
        {
            // FluentValidation testas automatiskt här eftersom det körs i pipelinen
            var command = new RegisterCommand("inte-en-email", "pass", "", "");

            var response = await _client.PostAsJsonAsync("/api/v1/auth/register", command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
