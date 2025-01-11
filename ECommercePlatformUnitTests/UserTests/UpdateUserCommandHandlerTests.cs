using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommercePlatformUnitTests.UserTests
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateUserCommandHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingUpdateUser_ThenShouldCallUpdateUserAsync()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Password = "password",
                Address = "123 Test St",
                PhoneNumber = "123-456-7890"
            };

            var user = new User
            {
                UserId = command.UserId,
                Name = command.Name,
                Email = command.Email,
                Password = command.Password,
                Address = command.Address,
                PhoneNumber = command.PhoneNumber
            };

            _mapper.Map<User>(command).Returns(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).UpdateUserAsync(user);
            result.Should().Be(Unit.Value);
        }
    }
}








