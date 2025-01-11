using Domain.Services;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Net.Mail;

namespace InfrastructureTests
{
    public class EmailServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _configuration = Substitute.For<IConfiguration>();
            _configuration["Smtp:Host"].Returns("smtp.example.com");
            _configuration["Smtp:Port"].Returns("587");
            _configuration["Smtp:Username"].Returns("username");
            _configuration["Smtp:Password"].Returns("password");
            _configuration["Smtp:EnableSsl"].Returns("true");
            _configuration["Smtp:From"].Returns("no-reply@example.com");

            _emailService = new EmailService(_configuration);
        }

        [Fact]
        public async Task GivenInvalidSmtpConfiguration_WhenSendingEmail_ThenShouldThrowException()
        {
            // Arrange
            _configuration["Smtp:Host"].Returns("invalid.smtp.example.com");

            var emailService = new EmailService(_configuration);
            var to = "recipient@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            Func<Task> act = async () => await emailService.SendEmailAsync(to, subject, body);

            // Assert
            await act.Should().ThrowAsync<SmtpException>();
        }

        [Fact]
        public async Task GivenInvalidEmailAddress_WhenSendingEmail_ThenShouldThrowException()
        {
            // Arrange
            var to = "invalid-email";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            Func<Task> act = async () => await _emailService.SendEmailAsync(to, subject, body);

            // Assert
            await act.Should().ThrowAsync<FormatException>();
        }
    }
}