using Application.UseCases.Queries.UserQueries;
using FluentAssertions;

namespace ECommercePlatformUnitTests.UserTests;

public class GetAllUsersQueryTests
{
    [Fact]
    public void GivenNoParameters_WhenCreatingGetAllUsersQuery_ThenQueryShouldBeInitialized()
    {
        // Arrange & Act
        var query = new GetAllUsersQuery();

        // Assert
        query.Should().NotBeNull();
    }
}