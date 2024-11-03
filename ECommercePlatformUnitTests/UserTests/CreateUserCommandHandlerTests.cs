using Application.UseCases.CommandHandlers.UserCommandHandlers;
using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.UserTests;

public class CreateUserCommandHandlerTests
{
    private readonly CreateUserCommandHandler _handler;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateUserCommandHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidCommand_WhenHandlingCreateUser_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "John Doe",
            Email = "john@example.com",
            Password = "password123",
            Address = "123 Main St",
            PhoneNumber = "1234567890"
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
    public async Task GivenInvalidCommand_WhenHandlingCreateUser_ThenShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "", // Invalid empty name
            Email = "invalid-email",
            Password = "",
            Address = "",
            PhoneNumber = ""
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenMaxLengthValues_WhenCreatingUser_ThenShouldSucceed()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = new string('A', 200),
            Email = new string('a', 100) + "@test.com",
            Password = "ValidPassword123!",
            Address = new string('A', 200),
            PhoneNumber = "1234567890"
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

    [Theory]
    [InlineData("test@test.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("@test.com", false)]
    [InlineData("test@", false)]
    [InlineData("test.com", false)]
    public async Task GivenVariousEmailFormats_WhenCreatingUser_ThenShouldValidateCorrectly(string email,
        bool shouldSucceed)
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "Test User",
            Email = email,
            Password = "ValidPassword123!",
            Address = "Test Address",
            PhoneNumber = "1234567890"
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

        if (shouldSucceed)
        {
            _mapper.Map<User>(command).Returns(user);
            _userRepository.AddUserAsync(user).Returns(Result<Guid>.Success(user.UserId));
        }

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(shouldSucceed);
        if (shouldSucceed) result.Data.Should().Be(user.UserId);
    }

    [Theory]
    [InlineData("Password123!", true)] // Valid password
    [InlineData("short", false)] // Too short
    [InlineData("NoNumbers!", false)] // No numbers
    [InlineData("no-uppercase1", false)] // No uppercase
    [InlineData("NO-LOWERCASE1", false)] // No lowercase
    public async Task GivenVariousPasswords_WhenCreatingUser_ThenShouldValidateCorrectly(string password,
        bool shouldSucceed)
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "Test User",
            Email = "test@test.com",
            Password = password,
            Address = "Test Address",
            PhoneNumber = "1234567890"
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

        if (shouldSucceed)
        {
            _mapper.Map<User>(command).Returns(user);
            _userRepository.AddUserAsync(user).Returns(Result<Guid>.Success(user.UserId));
        }

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(shouldSucceed);
        if (shouldSucceed) result.Data.Should().Be(user.UserId);
    }
}