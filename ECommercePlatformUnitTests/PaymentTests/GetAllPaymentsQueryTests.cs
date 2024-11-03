using Application.UseCases.Queries.PaymentQueries;
using FluentAssertions;

namespace ECommercePlatformUnitTests.PaymentTests;

public class GetAllPaymentsQueryTests
{
    [Fact]
    public void GivenNoParameters_WhenCreatingGetAllPaymentsQuery_ThenQueryShouldBeInitialized()
    {
        // Arrange & Act
        var query = new GetAllPaymentsQuery();

        // Assert
        query.Should().NotBeNull();
    }
}