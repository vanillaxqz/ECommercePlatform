using Application.UseCases.Queries;
using FluentAssertions;

namespace ECommercePlatformUnitTests.OrderTests;

public class GetAllOrdersQueryTests
{
    [Fact]
    public void GivenNoParameters_WhenCreatingGetAllOrdersQuery_ThenQueryShouldBeInitialized()
    {
        // Arrange & Act
        var query = new GetAllOrdersQuery();

        // Assert
        query.Should().NotBeNull();
    }
}