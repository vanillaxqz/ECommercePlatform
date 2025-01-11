using Application.UseCases.Commands.PaymentCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.PaymentTests
{
    public class UpdatePaymentCommandValidatorTests
    {
        private readonly UpdatePaymentCommandValidator _validator;

        public UpdatePaymentCommandValidatorTests()
        {
            _validator = new UpdatePaymentCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new UpdatePaymentCommand
            {
                PaymentId = Guid.NewGuid(),
                PaymentDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidPaymentId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdatePaymentCommand
            {
                PaymentId = Guid.Empty,
                PaymentDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PaymentId)
                .WithErrorMessage("Must be a valid payment guid");
        }

        [Fact]
        public void GivenInvalidUserId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdatePaymentCommand
            {
                PaymentId = Guid.NewGuid(),
                PaymentDate = DateTime.UtcNow,
                UserId = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserId)
                .WithErrorMessage("Must be a valid user guid");
        }

        [Fact]
        public void GivenEmptyPaymentDate_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdatePaymentCommand
            {
                PaymentId = Guid.NewGuid(),
                PaymentDate = default,
                UserId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PaymentDate);
        }
    }
}