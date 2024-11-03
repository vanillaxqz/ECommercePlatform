using Application.UseCases.Queries.UserQueries;
using Application.UseCases.QueryHandlers.UserQueryHandlers;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.UserTests;

public class GetAllUsersQueryHandlerTests
{
    private readonly GetAllUsersQueryHandler _handler;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetAllUsersQueryHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task GivenMultipleUsers_WhenGetAllUsersQueryIsHandled_ThenAllUsersShouldBeReturned()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password123",
                Address = "123 Main St",
                PhoneNumber = "1234567890"
            },
            new()
            {
                UserId = Guid.NewGuid(),
                Name = "Jane Smith",
                Email = "jane@example.com",
                Password = "password456",
                Address = "456 Oak St",
                PhoneNumber = "0987654321"
            }
        };

        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().AllSatisfy(u =>
        {
            u.UserId.Should().NotBe(Guid.Empty);
            u.Name.Should().NotBeNullOrEmpty();
            u.Email.Should().NotBeNullOrEmpty();
            u.Password.Should().NotBeNullOrEmpty();
            u.Address.Should().NotBeNullOrEmpty();
            u.PhoneNumber.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GivenNoUsers_WhenGetAllUsersQueryIsHandled_ThenShouldReturnEmptyList()
    {
        // Arrange
        _userRepository.GetAllUsersAsync()
            .Returns(Result<IEnumerable<User>>.Success(new List<User>()));

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}