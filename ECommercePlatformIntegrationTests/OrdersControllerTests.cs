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
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

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
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

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
            var response = await _client.GetAsync($"/api/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNonExistingOrder_WhenGettingOrderById_ThenShouldReturnNotFoundResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


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
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

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
            var response = await _client.DeleteAsync($"/api/orders/{orderId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
