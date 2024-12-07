using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net.Http.Headers;
using Infrastructure;
using System.IdentityModel.Tokens.Jwt;

namespace ECommercePlatformIntegrationTests
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _client;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContext.Database.EnsureCreated();
            _client = _factory.CreateClient();
            var tokenHandler = new JwtTokenGenerator("3fdd5f93-4ddb-465e-a2e8-3e326175030f");
            var token = tokenHandler.GenerateAccessToken(new Guid("3fdd5f93-4ddb-465e-a2e8-3e326175030f"), "testemail@gmail.com");
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Trait("Category", "ExcludeThis")]
        [Fact]
        public async Task GivenOrdersExist_WhenGettingAllOrders_ThenShouldReturnOkResponse()
        {
            // Arrange
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/v1/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Trait("Category", "ExcludeThis")]
        [Fact]
        public async Task GivenExistingOrder_WhenGettingOrderById_ThenShouldReturnOkResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // Act
            var response = await _client.GetAsync($"/api/v1/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Trait("Category", "ExcludeThis")]
        [Fact]
        public async Task GivenNonExistingOrder_WhenGettingOrderById_ThenShouldReturnNotFoundResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/v1/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Trait("Category", "ExcludeThis")]
        [Fact]
        public async Task GivenValidOrderRequest_WhenCreatingOrder_ThenShouldReturnCreatedResponse()
        {
            // Arrange
            var newOrder = new OrderDto
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };
            var content = new StringContent(JsonSerializer.Serialize(newOrder), Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/api/v1/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Trait("Category", "ExcludeThis")]
        [Fact]
        public async Task GivenExistingOrder_WhenDeletingOrder_ThenShouldReturnNoContentResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // Act
            var response = await _client.DeleteAsync($"/api/v1/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
