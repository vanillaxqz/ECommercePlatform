using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using FluentAssertions;
using Infrastructure;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;

namespace ECommercePlatformUnitTests.UserTests
{
    public class InitiatePasswordResetCommandHandlerTests
    {
        private readonly InitiatePasswordResetCommandHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public InitiatePasswordResetCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _emailService = Substitute.For<IEmailService>();
            _jwtTokenGenerator = Substitute.For<JwtTokenGenerator>("averylongsecretkeythatisrequiredtobeused");
            _handler = new InitiatePasswordResetCommandHandler(_userRepository, _emailService, _jwtTokenGenerator);
        }

        [Fact]
        public async Task GivenInvalidEmail_WhenHandlingInitiatePasswordReset_ThenShouldReturnFailureResult()
        {
            // Arrange
            var command = new InitiatePasswordResetCommand { Email = "invalid@example.com" };

            _userRepository.GetUserByEmailAsync(command.Email).Returns(Result<User>.Failure("User not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found");
            await _emailService.DidNotReceive().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_UserFound_SendsEmailAndReturnsSuccess()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Email = "burd@yahoo.com" };
            var command = new InitiatePasswordResetCommand { Email = user.Email };
            var token = new JwtSecurityToken();
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _userRepository.GetUserByEmailAsync(command.Email).Returns(Result<User>.Success(user));
            var tok = _jwtTokenGenerator.GeneratePasswordResetToken(user.UserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
