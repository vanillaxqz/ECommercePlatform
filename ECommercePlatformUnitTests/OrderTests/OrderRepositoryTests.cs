using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.OrderTests;

public class OrderRepositoryTests
{
    private readonly IOrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
    }

    [Fact]
    public async Task GivenValidOrder_WhenAddingOrder_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { OrderId = orderId, UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow };

        _orderRepository.AddOrderAsync(order).Returns(Result<Guid>.Success(orderId));

        // Act
        var result = await _orderRepository.AddOrderAsync(order);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(orderId);
    }

    [Fact]
    public async Task GivenExistingOrders_WhenGettingAllOrders_ThenShouldReturnAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow },
            new() { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow }
        };

        _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));

        // Act
        var result = await _orderRepository.GetAllOrdersAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GivenExistingOrder_WhenGettingOrderById_ThenShouldReturnMatchingOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { OrderId = orderId, UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(Result<Order>.Success(order));

        // Act
        var result = await _orderRepository.GetOrderByIdAsync(orderId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(order);
    }

    [Theory]
    [InlineData(Status.Pending)]
    [InlineData(Status.Shipped)]
    [InlineData(Status.Completed)]
    public async Task GivenDifferentStatuses_WhenAddingOrder_ThenShouldSucceed(Status status)
    {
        // Arrange
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = status,
            PaymentId = Guid.NewGuid()
        };

        _orderRepository.AddOrderAsync(order).Returns(Result<Guid>.Success(order.OrderId));

        // Act
        var result = await _orderRepository.AddOrderAsync(order);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(order.OrderId);
    }

    [Fact]
    public async Task GivenUpdateOrder_WhenUpdatingStatus_ThenShouldUpdateSuccessfully()
    {
        // Arrange
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.NewGuid()
        };

        // Act
        await _orderRepository.UpdateOrderAsync(order);

        // Initial status should be Pending
        order.Status = Status.Shipped;
        await _orderRepository.UpdateOrderAsync(order);

        // Update to Shipped
        order.Status = Status.Completed;
        await _orderRepository.UpdateOrderAsync(order);

        // Assert
        await _orderRepository.Received(3).UpdateOrderAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task GivenOrderWithInvalidPaymentId_WhenAdding_ThenShouldFail()
    {
        // Arrange
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.Empty // Invalid payment ID
        };

        _orderRepository.AddOrderAsync(order).Returns(Result<Guid>.Failure("Invalid payment ID"));

        // Act
        var result = await _orderRepository.AddOrderAsync(order);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}