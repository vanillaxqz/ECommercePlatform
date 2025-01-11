using Application.UseCases.Commands.ProductCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.ProductTests
{
    public class DeleteProductCommandValidatorTests
    {
        private readonly DeleteProductCommandValidator _validator;

        public DeleteProductCommandValidatorTests()
        {
            _validator = new DeleteProductCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new DeleteProductCommand
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
            var command = new DeleteProductCommand
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