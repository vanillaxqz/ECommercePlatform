using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using FluentAssertions;
using Infrastructure;
using NSubstitute;

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
    }
}
