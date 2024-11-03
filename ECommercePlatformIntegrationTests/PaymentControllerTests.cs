using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Application.DTOs;

namespace ECommercePlatformIntegrationTests
{
    public class PaymentsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PaymentsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPayments_ShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("/api/payments");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPaymentById_NonExistingPayment_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/payments/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreatePayment_ValidRequest_ShouldReturnCreated()
        {
            var payment = new PaymentDto
            {
                PaymentId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PaymentDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/payments", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}