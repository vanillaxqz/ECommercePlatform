using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using MediatR;

namespace ECommercePlatformUnitTests.UserTests
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly DeleteUserCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeleteUserCommandHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingDeleteUser_ThenShouldCallDeleteUserAsync()
        {
            // Arrange
            var command = new DeleteUserCommand { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).DeleteUserAsync(command.Id);
            result.Should().Be(Unit.Value);
        }
    }
}

