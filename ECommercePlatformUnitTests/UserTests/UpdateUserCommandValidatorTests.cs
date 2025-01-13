using Application.UseCases.Commands.UserCommands;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.UserTests
{
    public class UpdateUserCommandValidatorTests
    {
        private readonly UpdateUserCommandValidator _validator;

        public UpdateUserCommandValidatorTests()
        {
            _validator = new UpdateUserCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidUserId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.Empty,
                Name = "Test User",
                Email = "test@example.com",
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserId)
                .WithErrorMessage("Must be a valid guid");
        }

        [Fact]
        public void GivenEmptyName_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = string.Empty,
                Email = "test@example.com",
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public void GivenEmptyEmail_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = string.Empty,
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public void GivenInvalidEmail_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "invalid-email",
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("Invalid email address");
        }

        [Fact]
        public void GivenEmptyAddress_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Address = string.Empty,
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public void GivenEmptyPhoneNumber_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Address = "123 Test St",
                PhoneNumber = string.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }

        [Fact]
        public void GivenInvalidPhoneNumber_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Address = "123 Test St",
                PhoneNumber = "12345678901" // More than 10 characters
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber)
                .WithErrorMessage("The length of 'Phone Number' must be 10 characters or fewer. You entered 11 characters.");
        }
    }
}