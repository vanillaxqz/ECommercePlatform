using Application.UseCases.Commands.UserCommands;
using FluentAssertions;

namespace ECommercePlatformUnitTests.UserTests;

public class CreateUserCommandTests
{
    [Fact]
    public void GivenUserProperties_WhenCreatingNewCommand_ThenPropertiesShouldBeSetCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        var password = "password123";
        var address = "123 Main St";
        var phoneNumber = "1234567890";

        // Act
        var command = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password,
            Address = address,
            PhoneNumber = phoneNumber
        };

        // Assert
        command.Name.Should().Be(name);
        command.Email.Should().Be(email);
        command.Password.Should().Be(password);
        command.Address.Should().Be(address);
        command.PhoneNumber.Should().Be(phoneNumber);
    }
}