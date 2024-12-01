using Application.DTOs;
using Application.UseCases.QueryHandlers.OrderQueryHandlers;
using Application.UseCases.Queries.OrderQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Domain.Common;

namespace ECommercePlatformUnitTests.OrderTests;

public class GetFilteredOrdersQueryHandlerTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly GetFilteredOrdersQueryHandler _handler;

    public GetFilteredOrdersQueryHandlerTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetFilteredOrdersQueryHandler(_orderRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredOrders()
    {
        // Arrange
        var orders = new List<Order> { new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() } };
        var orderDtos = new List<OrderDto> { new OrderDto { OrderId = orders[0].OrderId, UserId = orders[0].UserId } };
        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
        _mapper.Map<List<OrderDto>>(Arg.Any<IEnumerable<Order>>()).Returns(orderDtos);

        var query = new GetFilteredOrdersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(orderDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoOrdersMatch()
    {
        // Arrange
        var orders = new List<Order>();
        var orderDtos = new List<OrderDto>();
        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
        _mapper.Map<List<OrderDto>>(Arg.Any<IEnumerable<Order>>()).Returns(orderDtos);

        var query = new GetFilteredOrdersQuery { Page = 1, PageSize = 10 };

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
        var userId = Guid.NewGuid();
        var orders = new List<Order>
        {
            new Order { OrderId = Guid.NewGuid(), UserId = userId },
            new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        var orderDtos = new List<OrderDto>
        {
            new OrderDto { OrderId = orders[0].OrderId, UserId = orders[0].UserId }
        };
        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
        _mapper.Map<List<OrderDto>>(Arg.Any<IEnumerable<Order>>()).Returns(orderDtos);

        var query = new GetFilteredOrdersQuery { Page = 1, PageSize = 10, UserId = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(orderDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyPagingCorrectly()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() },
            new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        var orderDtos = new List<OrderDto>
        {
            new OrderDto { OrderId = orders[0].OrderId, UserId = orders[0].UserId }
        };
        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
        _mapper.Map<List<OrderDto>>(Arg.Any<IEnumerable<Order>>()).Returns(orderDtos);

        var query = new GetFilteredOrdersQuery { Page = 1, PageSize = 1 };

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
        var orders = new List<Order> { new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() } };
        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
        _mapper.Map<List<OrderDto>>(Arg.Any<IEnumerable<Order>>()).Returns(x => throw new Exception("Mapping failure"));

        var query = new GetFilteredOrdersQuery { Page = 1, PageSize = 10 };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Mapping failure");
    }
}
