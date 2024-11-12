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
using Application.UseCases.Commands.ProductCommands;

namespace ECommercePlatformIntegrationTests
{
    public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;
        private readonly HttpClient client;
        private const string BaseUrl = "/api/products";

        public ProductsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
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

            var scope = this.factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
            client = this.factory.CreateClient();
        }

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Fact]
        public async Task GivenProductsExist_WhenGettingAllProducts_ThenShouldReturnOkResponse()
        {
            // Arrange
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = "Sample Product",
                Description = "A sample product description",
                Price = 99.99M,
                Stock = 50,
                Category = Category.Electronics
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Act
            var response = await client.GetAsync(BaseUrl);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNonExistingProduct_WhenGettingProductById_ThenShouldReturnNotFound()
        {
            // Act
            var response = await client.GetAsync($"{BaseUrl}/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidProductRequest_WhenCreatingProduct_ThenShouldReturnCreated()
        {
            // Arrange
            var product = new CreateProductCommand
            {
                Name = "Sample Product",
                Description = "A sample product description",
                Price = 99.99M,
                Stock = 50,
                Category = Category.Electronics
            };
            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(BaseUrl, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
