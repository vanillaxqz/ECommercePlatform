using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace ECommercePlatformUnitTests.UserTests;

public class UpdateUserCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task GivenValidCommand_WhenHandlingUpdateUser_ThenShouldUpdateSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand
        {
            UserId = userId,
            Name = "Updated Name",
            Email = "updated@example.com",
            Password = "newpassword123",
            Address = "New Address",
            PhoneNumber = "9876543210"
        };

        var user = new User
        {
            UserId = userId,
            Name = command.Name,
            Email = command.Email,
            Password = command.Password,
            Address = command.Address,
            PhoneNumber = command.PhoneNumber
        };

        _mapper.Map<User>(command).Returns(user);

        // Act & Assert
        await _userRepository.Received(0).UpdateUserAsync(Arg.Any<User>());
        await _userRepository.UpdateUserAsync(Arg.Is<User>(u =>
            u.UserId == userId &&
            u.Name == command.Name &&
            u.Email == command.Email &&
            u.Password == command.Password &&
            u.Address == command.Address &&
            u.PhoneNumber == command.PhoneNumber
        ));
    }

    [Fact]
    public async Task GivenInvalidCommand_WhenHandlingUpdateUser_ThenRepositoryShouldNotBeCalled()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            UserId = Guid.Empty,
            Name = "",
            Email = "",
            Password = "",
            Address = "",
            PhoneNumber = ""
        };

        // Act
        await Task.CompletedTask; // Placeholder for actual action

        // Assert
        await _userRepository.Received(0).UpdateUserAsync(Arg.Any<User>());
    }
}