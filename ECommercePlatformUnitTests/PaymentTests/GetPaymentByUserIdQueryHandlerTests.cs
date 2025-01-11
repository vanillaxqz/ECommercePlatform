using Application.DTOs;
using Application.UseCases.QueryHandlers.PaymentQueryHandlers;
using Application.UseCases.Queries.PaymentQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.PaymentTests
{
    public class GetPaymentByUserIdQueryHandlerTests
    {
        private readonly GetPaymentByUserIdQueryHandler _handler;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public GetPaymentByUserIdQueryHandlerTests()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetPaymentByUserIdQueryHandler(_paymentRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidUserId_WhenHandlingGetPaymentByUserIdQuery_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var payments = new List<Payment>
            {
                new Payment { PaymentId = Guid.NewGuid(), UserId = userId, PaymentDate = DateTime.UtcNow }
            };
            var paymentsDto = new List<PaymentDto>
            {
                new PaymentDto { PaymentId = payments[0].PaymentId, UserId = userId, PaymentDate = payments[0].PaymentDate }
            };

            _paymentRepository.GetPaymentsByUserIdAsync(userId).Returns(Result<IEnumerable<Payment>>.Success(payments));
            _mapper.Map<IEnumerable<PaymentDto>>(payments).Returns(paymentsDto);

            var query = new GetPaymentByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenInvalidUserId_WhenHandlingGetPaymentByUserIdQuery_ThenShouldReturnFailureResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var errorMessage = "Failure";

            _paymentRepository.GetPaymentsByUserIdAsync(userId).Returns(Result<IEnumerable<Payment>>.Failure(errorMessage));

            var query = new GetPaymentByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenNoPayments_WhenHandlingGetPaymentByUserIdQuery_ThenShouldReturnNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var payments = new List<Payment>();

            _paymentRepository.GetPaymentsByUserIdAsync(userId).Returns(Result<IEnumerable<Payment>>.Success(payments));

            var query = new GetPaymentByUserIdQuery { Id = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}