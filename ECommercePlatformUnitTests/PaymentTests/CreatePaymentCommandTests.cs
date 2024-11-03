using Application.UseCases.Commands.PaymentCommands;
using FluentAssertions;

namespace ECommercePlatformUnitTests.PaymentTests;

public class CreatePaymentCommandTests
{
    [Fact]
    public void GivenPaymentProperties_WhenCreatingNewCommand_ThenPropertiesShouldBeSetCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var paymentDate = DateTime.UtcNow;

        // Act
        var command = new CreatePaymentCommand
        {
            UserId = userId,
            PaymentDate = paymentDate
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.PaymentDate.Should().Be(paymentDate);
    }
}