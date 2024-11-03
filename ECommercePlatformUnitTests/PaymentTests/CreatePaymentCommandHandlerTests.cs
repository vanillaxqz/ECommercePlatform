using Application.UseCases.CommandHandlers.PaymentCommandHandlers;
using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.PaymentTests;

public class CreatePaymentCommandHandlerTests
{
    private readonly CreatePaymentCommandHandler _handler;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandlerTests()
    {
        _paymentRepository = Substitute.For<IPaymentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreatePaymentCommandHandler(_paymentRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidCommand_WhenHandlingCreatePayment_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.UtcNow
        };

        var payment = new Payment { PaymentId = Guid.NewGuid(), UserId = command.UserId };
        _mapper.Map<Payment>(command).Returns(payment);
        _paymentRepository.AddPaymentAsync(payment).Returns(Result<Guid>.Success(payment.PaymentId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(payment.PaymentId);
    }

    [Fact]
    public async Task GivenInvalidCommand_WhenHandlingCreatePayment_ThenShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            UserId = Guid.Empty,
            PaymentDate = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}