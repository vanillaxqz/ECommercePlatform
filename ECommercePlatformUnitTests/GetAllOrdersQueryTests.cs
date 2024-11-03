using FluentAssertions;
using Application.UseCases.Queries;

namespace ECommercePlatformUnitTests
{
    public class GetAllOrdersQueryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var query = new GetAllOrdersQuery();

            // Assert
            query.Should().NotBeNull();
        }
    }
}