using FluentAssertions;
using NSubstitute;
using Application.UseCases.QueryHandlers.Order;
using Application.UseCases.Queries;
using Domain.Repositories;
using Domain.Common;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace ECommercePlatformUnitTests
{
    public class GetAllOrdersQueryHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly GetAllOrdersQueryHandler _handler;

        public GetAllOrdersQueryHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllOrdersQueryHandler(_orderRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() },
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid() }
            };

            var orderDtos = new List<OrderDto>
            {
                new OrderDto { OrderId = orders[0].OrderId, UserId = orders[0].UserId, OrderDate = DateTime.UtcNow, Status = Status.Completed, PaymentId = Guid.NewGuid() },
                new OrderDto { OrderId = orders[1].OrderId, UserId = orders[1].UserId, OrderDate = DateTime.UtcNow, Status = Status.Pending, PaymentId = Guid.NewGuid() }
            };

            _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));
            _mapper.Map<IEnumerable<OrderDto>>(orders).Returns(orderDtos);

            // Act
            var result = await _handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(orderDtos);
        }
    }
}
