using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PaymentQueries
{
    public class GetPaymentByUserIdQuery : IRequest<Result<IEnumerable<PaymentDto>>>
    {
        public Guid Id { get; set; }
    }
}
