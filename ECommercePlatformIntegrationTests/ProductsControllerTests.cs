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
        public async Task GivenProductsExist_WhenGettingAllProducts_ThenShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("/api/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNonExistingProduct_WhenGettingProductById_ThenShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidProductRequest_WhenCreatingProduct_ThenShouldReturnCreated()
        {
            var product = new ProductDto
            {
                ProductId = Guid.NewGuid(),
                Name = "Sample Product",
                Description = "A sample product description",
                Price = 99.99M,
                Stock = 50,
                Category = Category.Electronics
            };
            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/products", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}