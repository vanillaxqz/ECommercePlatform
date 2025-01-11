using Application.UseCases.Commands.PaymentCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.PaymentTests
{
    public class DeletePaymentCommandValidatorTests
    {
        private readonly DeletePaymentCommandValidator _validator;

        public DeletePaymentCommandValidatorTests()
        {
            _validator = new DeletePaymentCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new DeletePaymentCommand
            {
                Id = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new DeletePaymentCommand
            {
                Id = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("Must be a valid guid");
        }
    }
}




