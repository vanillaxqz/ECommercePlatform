using Application.UseCases.CommandHandlers.OrderCommandHandlers;
using Application.UseCases.Commands.OrderCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.OrderTests;

public class CreateOrderCommandHandlerTests
{
    private readonly CreateOrderCommandHandler _handler;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateOrderCommandHandler(_orderRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidCommand_WhenHandlingCreateOrder_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            UserId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.NewGuid()
        };

        var order = new Order { OrderId = Guid.NewGuid(), UserId = command.UserId };
        _mapper.Map<Order>(command).Returns(order);
        _orderRepository.AddOrderAsync(order).Returns(Result<Guid>.Success(order.OrderId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(order.OrderId);
    }

    [Fact]
    public async Task GivenInvalidCommand_WhenHandlingCreateOrder_ThenShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            UserId = Guid.Empty,
            OrderDate = DateTime.UtcNow,
            Status = Status.Pending,
            PaymentId = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}