using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Application.DTOs;

namespace ECommercePlatformIntegrationTests
{
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("/api/users");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ShouldReturnCreated()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123",
                Address = "123 Main Street",
                PhoneNumber = "123456789"
            };
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/users", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}