using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests
{
    public class AdminTests
    {
        [Fact]
        public void Admin_ShouldSetAndGetProperties()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var name = "Admin Name";
            var email = "admin@example.com";
            var password = "password";

            // Act
            var admin = new Admin
            {
                AdminId = adminId,
                Name = name,
                Email = email,
                Password = password
            };

            // Assert
            admin.AdminId.Should().Be(adminId);
            admin.Name.Should().Be(name);
            admin.Email.Should().Be(email);
            admin.Password.Should().Be(password);
        }

        [Fact]
        public void Admin_ShouldAllowNullProperties()
        {
            // Arrange & Act
            var admin = new Admin
            {
                AdminId = Guid.NewGuid(),
                Name = null,
                Email = null,
                Password = null
            };

            // Assert
            admin.Name.Should().BeNull();
            admin.Email.Should().BeNull();
            admin.Password.Should().BeNull();
        }
    }
}