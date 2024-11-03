using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Application.DTOs;
using Domain.Entities;

namespace ECommercePlatformIntegrationTests
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("/api/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductById_NonExistingProduct_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProduct_ValidRequest_ShouldReturnCreated()
        {
            var product = new ProductDto
            {
                ProductId = Guid.NewGuid(),
                Name = "Sample Product",
                Description = "A sample product description",
                Price = 99.99M,
                Stock = 50,
                Category = Category.Electronics // Use the Category enum value
            };
            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/products", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}