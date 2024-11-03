using Xunit;
using FluentAssertions;
using Application.UseCases.Commands.OrderCommands;
using Domain.Entities;

namespace ECommercePlatformUnitTests
{
    public class CreateOrderCommandTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orderDate = DateTime.UtcNow;
            var status = Status.Pending;
            var paymentId = Guid.NewGuid();

            // Act
            var command = new CreateOrderCommand
            {
                UserId = userId,
                OrderDate = orderDate,
                Status = status,
                PaymentId = paymentId
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.OrderDate.Should().Be(orderDate);
            command.Status.Should().Be(status);
            command.PaymentId.Should().Be(paymentId);
        }
    }
}