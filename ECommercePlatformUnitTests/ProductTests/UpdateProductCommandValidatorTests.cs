using Application.UseCases.Commands.ProductCommands;
using Domain.Entities;
using FluentValidation.TestHelper;

namespace ECommercePlatformUnitTests.ProductTests
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator;

        public UpdateProductCommandValidatorTests()
        {
            _validator = new UpdateProductCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0m,
                Stock = 10,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidProductId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.Empty,
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0m,
                Stock = 10,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ProductId)
                .WithErrorMessage("Must be a valid guid");
        }

        [Fact]
        public void GivenEmptyName_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = string.Empty,
                Description = "Test Description",
                Price = 100.0m,
                Stock = 10,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public void GivenEmptyDescription_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = string.Empty,
                Price = 100.0m,
                Stock = 10,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Description);
        }

        [Fact]
        public void GivenEmptyPrice_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 0,
                Stock = 10,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Price);
        }

        [Fact]
        public void GivenEmptyStock_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0m,
                Stock = 0,
                Category = Category.Electronics
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Stock);
        }

        [Fact]
        public void GivenEmptyCategory_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0m,
                Stock = 10,
                Category = 0 // Invalid category
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Category);
        }
    }
}