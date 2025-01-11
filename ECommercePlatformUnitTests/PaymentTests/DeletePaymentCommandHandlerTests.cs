using Application.UseCases.CommandHandlers.PaymentCommandHandlers;
using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace ECommercePlatformUnitTests.PaymentTests
{
    public class DeletePaymentCommandHandlerTests
    {
        private readonly DeletePaymentCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;

        public DeletePaymentCommandHandlerTests()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeletePaymentCommandHandler(_paymentRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingDeletePayment_ThenShouldCallDeletePaymentAsync()
        {
            // Arrange
            var command = new DeletePaymentCommand { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _paymentRepository.Received(1).DeletePaymentAsync(command.Id);
            result.Should().Be(Unit.Value);
        }
    }
}
