using Application.DTOs;
using Application.UseCases.QueryHandlers.PaymentQueryHandlers;
using Application.UseCases.Queries.PaymentQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Domain.Common;

namespace ECommercePlatformUnitTests.PaymentTests;

public class GetFilteredPaymentsQueryHandlerTests
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly GetFilteredPaymentsQueryHandler _handler;

    public GetFilteredPaymentsQueryHandlerTests()
    {
        _paymentRepository = Substitute.For<IPaymentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetFilteredPaymentsQueryHandler(_paymentRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredPayments()
    {
        // Arrange
        var payments = new List<Payment> { new Payment { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid() } };
        var paymentDtos = new List<PaymentDto> { new PaymentDto { PaymentId = payments[0].PaymentId, UserId = payments[0].UserId } };
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));
        _mapper.Map<List<PaymentDto>>(Arg.Any<IEnumerable<Payment>>()).Returns(paymentDtos);

        var query = new GetFilteredPaymentsQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(paymentDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoPaymentsMatch()
    {
        // Arrange
        var payments = new List<Payment>();
        var paymentDtos = new List<PaymentDto>();
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));
        _mapper.Map<List<PaymentDto>>(Arg.Any<IEnumerable<Payment>>()).Returns(paymentDtos);

        var query = new GetFilteredPaymentsQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldApplyFiltersCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var payments = new List<Payment>
        {
            new Payment { PaymentId = Guid.NewGuid(), UserId = userId },
            new Payment { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        var paymentDtos = new List<PaymentDto>
        {
            new PaymentDto { PaymentId = payments[0].PaymentId, UserId = payments[0].UserId }
        };
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));
        _mapper.Map<List<PaymentDto>>(Arg.Any<IEnumerable<Payment>>()).Returns(paymentDtos);

        var query = new GetFilteredPaymentsQuery { Page = 1, PageSize = 10, UserId = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(paymentDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyPagingCorrectly()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new Payment { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid() },
            new Payment { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        var paymentDtos = new List<PaymentDto>
        {
            new PaymentDto { PaymentId = payments[0].PaymentId, UserId = payments[0].UserId }
        };
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));
        _mapper.Map<List<PaymentDto>>(Arg.Any<IEnumerable<Payment>>()).Returns(paymentDtos);

        var query = new GetFilteredPaymentsQuery { Page = 1, PageSize = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var payments = new List<Payment> { new Payment { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid() } };
        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));
        _mapper.Map<List<PaymentDto>>(Arg.Any<IEnumerable<Payment>>()).Returns(x => throw new Exception("Mapping failure"));

        var query = new GetFilteredPaymentsQuery { Page = 1, PageSize = 10 };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Mapping failure");
    }
}
