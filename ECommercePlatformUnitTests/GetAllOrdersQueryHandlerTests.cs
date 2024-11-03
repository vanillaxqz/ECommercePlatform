using FluentAssertions;
using NSubstitute;
using Application.UseCases.QueryHandlers.Order;
using Application.UseCases.Queries;
using Domain.Repositories;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common;
using Application.Utils;

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
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _handler = new GetAllOrdersQueryHandler(_orderRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow, Status = Status.Pending, PaymentId = Guid.NewGuid() },
                new Order { OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow, Status = Status.Completed, PaymentId = Guid.NewGuid() }
            };
            _orderRepository.GetAllOrdersAsync().Returns(Result<IEnumerable<Order>>.Success(orders));

            var query = new GetAllOrdersQuery();

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Data.Should().ContainSingle(o => o.Status == Status.Pending);
            result.Data.Should().ContainSingle(o => o.Status == Status.Completed);
        }
    }
}