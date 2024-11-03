using FluentAssertions;
using NSubstitute;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;

namespace ECommercePlatformUnitTests
{
    public class OrderRepositoryTests
    {
        private readonly IOrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
        }

        [Fact]
        public async Task AddOrderAsync_ShouldReturnSuccessResult()
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
        public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow },
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow }
            };

            _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));

            // Act
            var result = await _orderRepository.GetAllOrdersAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder()
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
    }
}
