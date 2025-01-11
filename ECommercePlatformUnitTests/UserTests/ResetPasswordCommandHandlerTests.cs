using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommercePlatformUnitTests.UserTests
{
    public class ResetPasswordCommandHandlerTests
    {
        private readonly ResetPasswordCommandHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResetPasswordCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _handler = new ResetPasswordCommandHandler(_userRepository, _httpContextAccessor);
        }

        [Fact]
        public async Task GivenInvalidToken_WhenHandlingResetPassword_ThenShouldReturnFailureResult()
        {
            // Arrange
            var command = new ResetPasswordCommand { NewPassword = "newpassword" };

            _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid token");
            await _userRepository.DidNotReceive().UpdateUserAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task GivenValidToken_WhenUserNotFound_ThenShouldReturnFailureResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new ResetPasswordCommand { NewPassword = "newpassword" };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = claimsPrincipal });
            _userRepository.GetUserByIdAsync(userId).Returns(Result<User>.Failure("User not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found");
            await _userRepository.DidNotReceive().UpdateUserAsync(Arg.Any<User>());
        }
    }
}

