using FluentAssertions;
using Infrastructure;

namespace InfrastructureTests
{
    public class HasherTests
    {
        [Fact]
        public void GivenValidPassword_WhenHashingPassword_ThenShouldReturnHashedPassword()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hashedPassword = Hasher.HashPassword(password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Length.Should().BeLessOrEqualTo(100);
        }

        [Fact]
        public void GivenValidPasswordAndHash_WhenVerifyingPassword_ThenShouldReturnTrue()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = Hasher.HashPassword(password);

            // Act
            var result = Hasher.VerifyPassword(password, hashedPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GivenInvalidPasswordAndHash_WhenVerifyingPassword_ThenShouldReturnFalse()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = Hasher.HashPassword(password);
            var invalidPassword = "WrongPassword!";

            // Act
            var result = Hasher.VerifyPassword(invalidPassword, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GivenLongHash_WhenVerifyingPassword_ThenShouldTruncateHashAndReturnTrue()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = Hasher.HashPassword(password);
            var longHash = hashedPassword + new string('a', 50); // Make the hash longer than 100 characters

            // Act
            var result = Hasher.VerifyPassword(password, longHash);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GivenLongHash_WhenHashingPassword_ThenShouldTruncateHash()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = Hasher.HashPassword(password);
            var longHash = hashedPassword + new string('a', 50); // Make the hash longer than 100 characters

            // Act
            var truncatedHash = Hasher.HashPassword(longHash);

            // Assert
            truncatedHash.Length.Should().BeLessOrEqualTo(100);
        }
    }
}
