using Application.UseCases.CommandHandlers.OrderCommandHandlers;
using Application.UseCases.Commands.OrderCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace ECommercePlatformUnitTests.OrderTests;

public class UpdateOrderCommandHandlerTests
{
    private readonly IRequestHandler<UpdateOrderCommand, Unit> _handler;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandlerTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateOrderCommandHandler(_orderRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidOrder_WhenUpdating_ThenShouldSucceed()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new UpdateOrderCommand
        {
            OrderId = orderId,
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.NewGuid()
        };

        var order = new Order
        {
            OrderId = command.OrderId,
            UserId = command.UserId,
            OrderDate = command.OrderDate,
            Status = command.Status,
            PaymentId = command.PaymentId
        };

        _mapper.Map<Order>(command).Returns(order);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        await _orderRepository.Received(1).UpdateOrderAsync(Arg.Is<Order>(o =>
            o.OrderId == orderId &&
            o.UserId == command.UserId &&
            o.Status == command.Status
        ));
    }

    [Fact]
    public async Task GivenInvalidOrder_WhenUpdating_ThenShouldFail()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            OrderId = Guid.Empty,
            UserId = Guid.Empty,
            OrderDate = DateTime.MinValue,
            Status = 0,
            PaymentId = Guid.Empty
        };

        // Act & Assert
        await _orderRepository.Received(0).UpdateOrderAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task GivenNullMapper_WhenUpdating_ThenShouldFail()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.NewGuid()
        };

        _mapper.Map<Order>(command).Returns(null as Order);

        // Act & Assert
        await _orderRepository.Received(0).UpdateOrderAsync(Arg.Any<Order>());
    }
}