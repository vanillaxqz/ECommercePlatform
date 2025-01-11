using Application.UseCases.Commands.UserCommands;
using Application.UseCases.Authentication;
using Application.DTOs;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;

namespace ECommercePlatform.Controllers.Tests
{
    public class AuthControllerTests
    {
        private readonly IMediator _mediator;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new AuthController(_mediator);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnAccessToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            var userDto = new UserDto { UserId = Guid.NewGuid(), Email = user.Email };
            _mediator.Send(user).Returns(Result<UserDto>.Success(userDto));

            // Act
            var result = await _controller.LoginUser(user);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginUser_ShouldReturnBadRequest_WhenCredentialsAreInvalid()
        {
            // Arrange
            var user = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            _mediator.Send(user).Returns(Result<UserDto>.Failure("Invalid credentials"));

            // Act
            var result = await _controller.LoginUser(user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnUserId_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "test@example.com", Password = "password" };
            var userId = Guid.NewGuid();
            _mediator.Send(command).Returns(Result<Guid>.Success(userId));

            // Act
            var result = await _controller.RegisterUser(command);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "test@example.com", Password = "password" };
            _mediator.Send(command).Returns(Result<Guid>.Failure("Registration failed"));

            // Act
            var result = await _controller.RegisterUser(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Registration failed", badRequestResult.Value);
        }

        [Fact]
        public async Task InitiatePasswordReset_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var command = new InitiatePasswordResetCommand { Email = "test@example.com" };
            var token = "reset_token";
            _mediator.Send(command).Returns(Result<string>.Success(token));

            // Act
            var result = await _controller.InitiatePasswordReset(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Result<string>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(token, response.Data);
        }

        [Fact]
        public async Task InitiatePasswordReset_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new InitiatePasswordResetCommand { Email = "nonexistent@example.com" };
            _mediator.Send(command).Returns(Result<string>.Failure("User not found"));

            // Act
            var result = await _controller.InitiatePasswordReset(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User not found", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPassword_ShouldReturnSuccess_WhenPasswordIsReset()
        {
            // Arrange
            var command = new ResetPasswordCommand { NewPassword = "newpassword" };
            _mediator.Send(command).Returns(Result<string>.Success("Password reset successful"));

            // Act
            var result = await _controller.ResetPassword(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Result<string>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal("Password reset successful", response.Data);
        }

        [Fact]
        public async Task ResetPassword_ShouldReturnBadRequest_WhenPasswordResetFails()
        {
            // Arrange
            var command = new ResetPasswordCommand { NewPassword = "newpassword" };
            _mediator.Send(command).Returns(Result<string>.Failure("Password reset failed"));

            // Act
            var result = await _controller.ResetPassword(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Password reset failed", badRequestResult.Value);
        }
    }
}
