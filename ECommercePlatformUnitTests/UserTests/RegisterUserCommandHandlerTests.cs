using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.UserTests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly RegisterUserCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new RegisterUserCommandHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingRegisterUser_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password",
                Address = "123 Test St",
                PhoneNumber = "123-456-7890"
            };

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email,
                Password = command.Password,
                Address = command.Address,
                PhoneNumber = command.PhoneNumber
            };

            _mapper.Map<User>(command).Returns(user);
            _userRepository.AddUserAsync(user).Returns(Result<Guid>.Success(user.UserId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(user.UserId);
        }

        [Fact]
        public async Task GivenInvalidCommand_WhenHandlingRegisterUser_ThenShouldReturnFailureResult()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password",
                Address = "123 Test St",
                PhoneNumber = "123-456-7890"
            };

            _mapper.Map<User>(command).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failure");
        }
    }
}

