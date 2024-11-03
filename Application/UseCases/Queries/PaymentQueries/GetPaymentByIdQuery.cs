using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PaymentQueries
{
    public class GetPaymentByIdQuery : IRequest<Result<PaymentDto>>
    {
        public Guid Id { get; set; }
    }
}
