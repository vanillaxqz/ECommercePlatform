using Application.UseCases.Commands.UserCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.UserTests
{
    public class DeleteUserCommandValidatorTests
    {
        private readonly DeleteUserCommandValidator _validator;

        public DeleteUserCommandValidatorTests()
        {
            _validator = new DeleteUserCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new DeleteUserCommand
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
            var command = new DeleteUserCommand
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