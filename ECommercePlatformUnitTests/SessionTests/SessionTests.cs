using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests
{
    public class SessionTests
    {
        [Fact]
        public void Session_ShouldSetAndGetProperties()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;

            // Act
            var session = new Session
            {
                SessionId = sessionId,
                UserId = userId,
                StartTime = startTime
            };

            // Assert
            session.SessionId.Should().Be(sessionId);
            session.UserId.Should().Be(userId);
            session.StartTime.Should().Be(startTime);
        }

        [Fact]
        public void Session_ShouldAllowDefaultStartTime()
        {
            // Arrange & Act
            var session = new Session
            {
                SessionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartTime = default
            };

            // Assert
            session.StartTime.Should().Be(default(DateTime));
        }
    }
}