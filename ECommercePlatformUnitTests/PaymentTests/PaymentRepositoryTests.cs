using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.PaymentTests;

public class PaymentRepositoryTests
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentRepositoryTests()
    {
        _paymentRepository = Substitute.For<IPaymentRepository>();
    }

    [Fact]
    public async Task GivenValidPayment_WhenAddingPayment_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment { PaymentId = paymentId, UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow };

        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Success(paymentId));

        // Act
        var result = await _paymentRepository.AddPaymentAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paymentId);
    }

    [Fact]
    public async Task GivenExistingPayments_WhenGettingAllPayments_ThenShouldReturnAllPayments()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new() { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow },
            new() { PaymentId = Guid.NewGuid(), UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow }
        };

        _paymentRepository.GetAllPaymentsAsync().Returns(Result<IEnumerable<Payment>>.Success(payments));

        // Act
        var result = await _paymentRepository.GetAllPaymentsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GivenExistingPayment_WhenGettingPaymentById_ThenShouldReturnMatchingPayment()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment { PaymentId = paymentId, UserId = Guid.NewGuid(), PaymentDate = DateTime.UtcNow };

        _paymentRepository.GetPaymentByIdAsync(paymentId).Returns(Result<Payment>.Success(payment));

        // Act
        var result = await _paymentRepository.GetPaymentByIdAsync(paymentId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(payment);
    }

    [Fact]
    public async Task GivenExistingPayments_WhenGettingPaymentsByUserId_ThenShouldReturnUserPayments()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var payments = new List<Payment>
        {
            new() { PaymentId = Guid.NewGuid(), UserId = userId, PaymentDate = DateTime.UtcNow },
            new() { PaymentId = Guid.NewGuid(), UserId = userId, PaymentDate = DateTime.UtcNow }
        };

        _paymentRepository.GetPaymentsByUserIdAsync(userId).Returns(Result<IEnumerable<Payment>>.Success(payments));

        // Act
        var result = await _paymentRepository.GetPaymentsByUserIdAsync(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().AllSatisfy(p => p.UserId.Should().Be(userId));
    }

    [Fact]
    public async Task GivenPaymentWithFutureDate_WhenAdding_ThenShouldSucceed()
    {
        // Arrange
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.UtcNow.AddDays(1) // Future date
        };

        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Success(payment.PaymentId));

        // Act
        var result = await _paymentRepository.AddPaymentAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(payment.PaymentId);
    }

    [Fact]
    public async Task GivenPaymentWithPastDate_WhenAdding_ThenShouldSucceed()
    {
        // Arrange
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.UtcNow.AddDays(-1) // Past date
        };

        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Success(payment.PaymentId));

        // Act
        var result = await _paymentRepository.AddPaymentAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(payment.PaymentId);
    }

    [Fact]
    public async Task GivenInvalidUserId_WhenAddingPayment_ThenShouldFail()
    {
        // Arrange
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            UserId = Guid.Empty, // Invalid user ID
            PaymentDate = DateTime.UtcNow
        };

        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Failure("Invalid user ID"));

        // Act
        var result = await _paymentRepository.AddPaymentAsync(payment);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenPaymentWithMaxValues_WhenAdding_ThenShouldSucceed()
    {
        // Arrange
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.MaxValue
        };

        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Success(payment.PaymentId));

        // Act
        var result = await _paymentRepository.AddPaymentAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(payment.PaymentId);
    }
}