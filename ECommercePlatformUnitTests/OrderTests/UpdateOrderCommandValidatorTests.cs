using Application.UseCases.Commands.OrderCommands;
using Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace ECommercePlatformUnitTests.OrderTests
{
    public class UpdateOrderCommandValidatorTests
    {
        private readonly UpdateOrderCommandValidator _validator;

        public UpdateOrderCommandValidatorTests()
        {
            _validator = new UpdateOrderCommandValidator();
        }

        [Fact]
        public void GivenValidCommand_WhenValidating_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidOrderId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.Empty,
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OrderId)
                .WithErrorMessage("Must be a valid order guid");
        }

        [Fact]
        public void GivenInvalidUserId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.Empty,
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserId)
                .WithErrorMessage("Must be a valid user guid");
        }

        [Fact]
        public void GivenInvalidPaymentId_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = Status.Pending,
                PaymentId = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PaymentId)
                .WithErrorMessage("Must be a valid payment guid");
        }

        [Fact]
        public void GivenEmptyOrderDate_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = default,
                Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OrderDate);
        }

        [Fact]
        public void GivenEmptyStatus_WhenValidating_ThenShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = 0, // Invalid status
                PaymentId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Status);
        }
    }
}



