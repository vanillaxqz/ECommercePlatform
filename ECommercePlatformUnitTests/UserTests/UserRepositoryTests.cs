using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure;
using NSubstitute;
using Xunit;

namespace ECommercePlatformUnitTests.UserTests
{
    public class UserRepositoryTests
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UserRepositoryTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _jwtTokenGenerator = new JwtTokenGenerator("supersecretkeythatissufficientlylong");
        }

        [Fact]
        public async Task GivenValidUser_WhenAddingUser_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Email = "test@example.com", Password = "password123" };

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
                new() { UserId = Guid.NewGuid(), Email = "user1@example.com", Password = "password123" },
                new() { UserId = Guid.NewGuid(), Email = "user2@example.com", Password = "password123" }
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
            var user = new User { UserId = userId, Email = "test@example.com", Password = "password123" };

            _userRepository.GetUserByIdAsync(userId).Returns(Result<User>.Success(user));

            // Act
            var result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GivenExistingUser_WhenGettingUserByEmail_ThenShouldReturnMatchingUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { UserId = Guid.NewGuid(), Email = email, Password = "password123" };

            _userRepository.GetUserByEmailAsync(email).Returns(Result<User>.Success(user));

            // Act
            var result = await _userRepository.GetUserByEmailAsync(email);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GivenNonExistingUser_WhenGettingUserById_ThenShouldReturnFailure()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepository.GetUserByIdAsync(userId).Returns(Result<User>.Failure("Error retrieving user by ID"));

            // Act
            var result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Error retrieving user by ID");
        }

        [Fact]
        public async Task GivenNonExistingUser_WhenGettingUserByEmail_ThenShouldReturnFailure()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _userRepository.GetUserByEmailAsync(email).Returns(Result<User>.Failure("User not found"));

            // Act
            var result = await _userRepository.GetUserByEmailAsync(email);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found");
        }

        [Fact]
        public async Task GivenValidUser_WhenLoggingIn_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User { UserId = Guid.NewGuid(), Email = email, Password = Hasher.HashPassword(password) };

            _userRepository.LoginUser(Arg.Is<User>(u => u.Email == email && u.Password == password))
                .Returns(Result<User>.Success(user));

            var loginUser = new User { Email = email, Password = password };

            // Act
            var result = await _userRepository.LoginUser(loginUser);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GivenInvalidPassword_WhenLoggingIn_ThenShouldReturnFailure()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User { UserId = Guid.NewGuid(), Email = email, Password = Hasher.HashPassword(password) };

            _userRepository.LoginUser(Arg.Is<User>(u => u.Email == email && u.Password == "wrongpassword"))
                .Returns(Result<User>.Failure("Invalid email or password"));

            var loginUser = new User { Email = email, Password = "wrongpassword" };

            // Act
            var result = await _userRepository.LoginUser(loginUser);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid email or password");
        }

        [Fact]
        public async Task GivenNonExistingUser_WhenLoggingIn_ThenShouldReturnFailure()
        {
            // Arrange
            var loginUser = new User { Email = "nonexistent@example.com", Password = "password123" };

            _userRepository.LoginUser(loginUser).Returns(Result<User>.Failure("Invalid email or password"));

            // Act
            var result = await _userRepository.LoginUser(loginUser);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid email or password");
        }

        [Fact]
        public async Task GivenExistingUser_WhenDeletingUser_ThenShouldDeleteSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepository.DeleteUserAsync(userId).Returns(Task.CompletedTask);

            // Act
            await _userRepository.DeleteUserAsync(userId);

            // Assert
            await _userRepository.Received(1).DeleteUserAsync(userId);
        }

        [Fact]
        public async Task GivenExistingUser_WhenUpdatingUser_ThenShouldUpdateSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Email = "test@example.com", Password = "password123" };

            // Act
            await _userRepository.UpdateUserAsync(user);

            // Assert
            await _userRepository.Received(1).UpdateUserAsync(user);
        }

        [Fact]
        public void GivenUser_WhenGeneratingPasswordResetToken_ThenShouldReturnToken()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Email = "test@example.com", Password = "password123" };

            // Act
            var token = _jwtTokenGenerator.GeneratePasswordResetToken(user.UserId);

            // Assert
            token.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenDatabaseError_WhenAddingUser_ThenShouldReturnFailure()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Email = "test@example.com", Password = "password123" };

            _userRepository.AddUserAsync(user).Returns(Result<Guid>.Failure("Error adding user"));

            // Act
            var result = await _userRepository.AddUserAsync(user);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error adding user");
        }

        [Fact]
        public async Task GivenDatabaseError_WhenGettingAllUsers_ThenShouldReturnFailure()
        {
            // Arrange
            _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Failure("Error retrieving all users"));

            // Act
            var result = await _userRepository.GetAllUsersAsync();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error retrieving all users");
        }
    }
}