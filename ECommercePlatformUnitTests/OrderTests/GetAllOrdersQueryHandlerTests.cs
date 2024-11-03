using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers.Order;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.OrderTests;

public class GetAllOrdersQueryHandlerTests
{
    private readonly GetAllOrdersQueryHandler _handler;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersQueryHandlerTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetAllOrdersQueryHandler(_orderRepository, _mapper);
    }

    [Fact]
    public async Task GivenMultipleOrders_WhenGetAllOrdersQueryIsHandled_ThenAllOrdersShouldBeReturned()
    {
        // Arrange
        var orders = new List<Order>
        {
            new()
            {
                OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow, Status = Status.Pending,
                PaymentId = Guid.NewGuid()
            },
            new()
            {
                OrderId = Guid.NewGuid(), UserId = Guid.NewGuid(), OrderDate = DateTime.UtcNow,
                Status = Status.Completed, PaymentId = Guid.NewGuid()
            }
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