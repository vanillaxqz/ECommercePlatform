using Application.UseCases.CommandHandlers.PaymentCommandHandlers;
using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace ECommercePlatformUnitTests.PaymentTests;

public class UpdatePaymentCommandHandlerTests
{
    private readonly UpdatePaymentCommandHandler _handler;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public UpdatePaymentCommandHandlerTests()
    {
        _paymentRepository = Substitute.For<IPaymentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdatePaymentCommandHandler(_paymentRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidPayment_WhenUpdating_ThenShouldSucceed()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var command = new UpdatePaymentCommand
        {
            PaymentId = paymentId,
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.UtcNow
        };

        var payment = new Payment
        {
            PaymentId = command.PaymentId,
            UserId = command.UserId,
            PaymentDate = command.PaymentDate
        };

        _mapper.Map<Payment>(command).Returns(payment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        await _paymentRepository.Received(1).UpdatePaymentAsync(Arg.Is<Payment>(p =>
            p.PaymentId == paymentId &&
            p.UserId == command.UserId
        ));
    }

    [Fact]
    public async Task GivenInvalidPayment_WhenUpdating_ThenShouldFail()
    {
        // Arrange
        var command = new UpdatePaymentCommand
        {
            PaymentId = Guid.Empty,
            UserId = Guid.Empty,
            PaymentDate = DateTime.MinValue
        };

        // Act & Assert
        await _paymentRepository.Received(0).UpdatePaymentAsync(Arg.Any<Payment>());
    }

    [Fact]
    public async Task GivenNullMapper_WhenUpdating_ThenShouldFail()
    {
        // Arrange
        var command = new UpdatePaymentCommand
        {
            PaymentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentDate = DateTime.UtcNow
        };

        _mapper.Map<Payment>(command).Returns((Payment?)null);

        // Act & Assert
        await _paymentRepository.Received(0).UpdatePaymentAsync(Arg.Any<Payment>());
    }
}