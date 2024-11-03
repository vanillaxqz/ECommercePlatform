using FluentAssertions;
using NSubstitute;
using Application.UseCases.CommandHandlers.OrderCommandHandlers;
using Application.UseCases.Commands.OrderCommands;
using Domain.Repositories;
using Domain.Common;
using Domain.Entities;
using AutoMapper;

namespace ECommercePlatformUnitTests
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateOrderCommandHandler(_orderRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnSuccessResult()
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
        public async Task Handle_InvalidCommand_ShouldReturnFailureResult()
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
}
