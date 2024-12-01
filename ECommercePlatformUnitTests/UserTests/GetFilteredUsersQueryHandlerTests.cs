using Application.DTOs;
using Application.UseCases.QueryHandlers.UserQueryHandlers;
using Application.UseCases.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Domain.Common;

namespace ECommercePlatformUnitTests.UserTests;

public class GetFilteredUsersQueryHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly GetFilteredUserQueryHandler _handler;

    public GetFilteredUsersQueryHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetFilteredUserQueryHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredUsers()
    {
        // Arrange
        var users = new List<User> { new User { UserId = Guid.NewGuid(), Name = "User1" } };
        var userDtos = new List<UserDto> { new UserDto { UserId = users[0].UserId, Name = users[0].Name } };
        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));
        _mapper.Map<List<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(userDtos);

        var query = new GetFilteredUsersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(userDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoUsersMatch()
    {
        // Arrange
        var users = new List<User>();
        var userDtos = new List<UserDto>();
        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));
        _mapper.Map<List<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(userDtos);

        var query = new GetFilteredUsersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldApplyFiltersCorrectly()
    {
        // Arrange
        var userName = "User1";
        var users = new List<User>
        {
            new User { UserId = Guid.NewGuid(), Name = userName },
            new User { UserId = Guid.NewGuid(), Name = "User2" }
        };
        var userDtos = new List<UserDto>
        {
            new UserDto { UserId = users[0].UserId, Name = users[0].Name }
        };
        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));
        _mapper.Map<List<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(userDtos);

        var query = new GetFilteredUsersQuery { Page = 1, PageSize = 10, Name = userName };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(userDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyPagingCorrectly()
    {
        // Arrange
        var users = new List<User>
        {
            new User { UserId = Guid.NewGuid(), Name = "User1" },
            new User { UserId = Guid.NewGuid(), Name = "User2" }
        };
        var userDtos = new List<UserDto>
        {
            new UserDto { UserId = users[0].UserId, Name = users[0].Name }
        };
        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));
        _mapper.Map<List<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(userDtos);

        var query = new GetFilteredUsersQuery { Page = 1, PageSize = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var users = new List<User> { new User { UserId = Guid.NewGuid(), Name = "User1" } };
        _userRepository.GetAllUsersAsync().Returns(Result<IEnumerable<User>>.Success(users));
        _mapper.Map<List<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(x => throw new Exception("Mapping failure"));

        var query = new GetFilteredUsersQuery { Page = 1, PageSize = 10 };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Mapping failure");
    }
}
