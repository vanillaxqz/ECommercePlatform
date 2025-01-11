using Application.UseCases.Commands.OrderCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.OrderTests
{
    public class DeleteOrderCommandValidatorTests
    {
        private readonly DeleteOrderCommandValidator _validator;

        public DeleteOrderCommandValidatorTests()
        {
            _validator = new DeleteOrderCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new DeleteOrderCommand
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
            var command = new DeleteOrderCommand
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



