using Application.UseCases.Queries.ProductQueries;
using FluentAssertions;

namespace ECommercePlatformUnitTests.ProductTests;

public class GetAllProductsQueryTests
{
    [Fact]
    public void GivenNoParameters_WhenCreatingGetAllProductsQuery_ThenQueryShouldBeInitialized()
    {
        // Arrange & Act
        var query = new GetAllProductsQuery();

        // Assert
        query.Should().NotBeNull();
    }
}