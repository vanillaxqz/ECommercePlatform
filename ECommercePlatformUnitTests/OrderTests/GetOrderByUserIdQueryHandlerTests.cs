using Application.DTOs;
using Application.UseCases.QueryHandlers.OrderQueryHandlers;
using Application.UseCases.Queries.OrderQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommercePlatformUnitTests.OrderTests
{
    public class GetOrderByUserIdQueryHandlerTests
    {
        private readonly GetOrderByUserIdQueryHandler _handler;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByUserIdQueryHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetOrderByUserIdQueryHandler(_orderRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidUserId_WhenHandlingGetOrderByUserIdQuery_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = new List<Order>
            {
                new Order { OrderId = Guid.NewGuid(), UserId = userId, OrderDate = DateTime.UtcNow, Status = Status.Pending, PaymentId = Guid.NewGuid() }
            };
            var ordersDto = new List<OrderDto>
            {
                new OrderDto { OrderId = orders[0].OrderId, UserId = userId, OrderDate = orders[0].OrderDate, Status = orders[0].Status, PaymentId = orders[0].PaymentId }
            };

            _orderRepository.GetOrdersByUserIdAsync(userId).Returns(Result<IEnumerable<Order>>.Success(orders));
            _mapper.Map<IEnumerable<OrderDto>>(orders).Returns(ordersDto);

            var query = new GetOrderByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenInvalidUserId_WhenHandlingGetOrderByUserIdQuery_ThenShouldReturnFailureResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var errorMessage = "Failure";

            _orderRepository.GetOrdersByUserIdAsync(userId).Returns(Result<IEnumerable<Order>>.Failure(errorMessage));

            var query = new GetOrderByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenNoOrders_WhenHandlingGetOrderByUserIdQuery_ThenShouldReturnNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = new List<Order>();

            _orderRepository.GetOrdersByUserIdAsync(userId).Returns(Result<IEnumerable<Order>>.Success(orders));

            var query = new GetOrderByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}


