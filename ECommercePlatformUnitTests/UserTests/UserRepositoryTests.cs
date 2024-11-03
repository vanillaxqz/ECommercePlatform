using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.UserTests;

public class UserRepositoryTests
{
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
    }

    [Fact]
    public async Task GivenValidUser_WhenAddingUser_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Name = "John Doe",
            Email = "john@example.com",
            Password = "password123",
            Address = "123 Main St",
            PhoneNumber = "1234567890"
        };

        _userRepository.AddUserAsync(user).Returns(Result<Guid>.Success(userId));

        // Act
        var result = await _userRepository.AddUserAsync(user);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(userId);
    }

    [Fact]
    public async Task GivenExistingUsers_WhenGettingAllUsers_ThenShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password123",
                Address = "123 Main St",
                PhoneNumber = "1234567890"
            },
            new()
            {
                UserId = Guid.NewGuid(),
                Name = "Jane Smith",
                Email = "jane@example.com",
                Password = "password456",
                Address = "456 Oak St",
                PhoneNumber = "0987654321"
            }
        };

        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));

        // Act
        var result = await _userRepository.GetAllUsersAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GivenExistingUser_WhenGettingUserById_ThenShouldReturnMatchingUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Name = "John Doe",
            Email = "john@example.com",
            Password = "password123",
            Address = "123 Main St",
            PhoneNumber = "1234567890"
        };

        _userRepository.GetUserByIdAsync(userId).Returns(Result<User>.Success(user));

        // Act
        var result = await _userRepository.GetUserByIdAsync(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GivenNonExistentUserId_WhenGettingUserById_ThenShouldReturnFailure()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();
        var failureResult = Result<User>.Failure("User not found");
        _userRepository.GetUserByIdAsync(nonExistentUserId).Returns(failureResult);

        // Act
        var result = await _userRepository.GetUserByIdAsync(nonExistentUserId);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenUserToDelete_WhenDeletingUser_ThenShouldDeleteSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        await _userRepository.DeleteUserAsync(userId);

        // Assert
        await _userRepository.Received(1).DeleteUserAsync(userId);
    }

    [Fact]
    public async Task GivenUserToUpdate_WhenUpdatingUser_ThenShouldUpdateSuccessfully()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = "Updated Name",
            Email = "updated@example.com",
            Password = "updatedpass",
            Address = "Updated Address",
            PhoneNumber = "9876543210"
        };

        // Act
        await _userRepository.UpdateUserAsync(user);

        // Assert
        await _userRepository.Received(1).UpdateUserAsync(Arg.Is<User>(u =>
            u.UserId == user.UserId &&
            u.Name == user.Name &&
            u.Email == user.Email));
    }

    [Fact]
    public async Task GivenDuplicateEmail_WhenAddingUser_ThenShouldFail()
    {
        // Arrange
        var existingEmail = "existing@test.com";
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = "Test User",
            Email = existingEmail,
            Password = "password123",
            Address = "Test Address",
            PhoneNumber = "1234567890"
        };

        _userRepository.AddUserAsync(user)
            .Returns(Result<Guid>.Failure("Email already exists"));

        // Act
        var result = await _userRepository.AddUserAsync(user);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData("short")] // Too short
    [InlineData("no-number")] // No number
    [InlineData("NO-LOWERCASE")] // No lowercase
    [InlineData("no-uppercase")] // No uppercase
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public async Task GivenInvalidPasswordFormat_WhenAddingUser_ThenShouldFail(string password)
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@test.com",
            Password = password,
            Address = "Test Address",
            PhoneNumber = "1234567890"
        };

        _userRepository.AddUserAsync(user)
            .Returns(Result<Guid>.Failure("Invalid password format"));

        // Act
        var result = await _userRepository.AddUserAsync(user);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenMaxLengthValues_WhenAddingUser_ThenShouldSucceed()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = new string('A', 200), // Max length name
            Email = new string('a', 100) + "@test.com",
            Password = "ValidPassword123!",
            Address = new string('A', 200), // Max length address
            PhoneNumber = "1234567890"
        };

        _userRepository.AddUserAsync(user)
            .Returns(Result<Guid>.Success(user.UserId));

        // Act
        var result = await _userRepository.AddUserAsync(user);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(user.UserId);
    }
}