using Application.UseCases.Queries.PaymentQueries;
using Application.UseCases.QueryHandlers.PaymentQueryHandlers;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.PaymentTests;

public class GetAllPaymentsQueryHandlerTests
{
    private readonly GetAllPaymentsQueryHandler _handler;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public GetAllPaymentsQueryHandlerTests()
    {
        _paymentRepository = Substitute.For<IPaymentRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetAllPaymentsQueryHandler(_paymentRepository, _mapper);
    }

    [Fact]
    public async Task GivenMultiplePayments_WhenGetAllPaymentsQueryIsHandled_ThenAllPaymentsShouldBeReturned()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new() { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow },
            new() { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow }
        };
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));

        var query = new GetAllPaymentsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().AllSatisfy(p =>
        {
            p.PaymentId.Should().NotBe(Guid.Empty);
            p.UserId.Should().NotBe(Guid.Empty);
            p.PaymentDate.Should().NotBe(default);
        });
    }

    [Fact]
    public async Task GivenNoPayments_WhenGetAllPaymentsQueryIsHandled_ThenShouldReturnEmptyList()
    {
        // Arrange
        _paymentRepository.GetAllPaymentsAsync()
            .Returns(Result<IEnumerable<Payment>>.Success(new List<Payment>()));

        var query = new GetAllPaymentsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}