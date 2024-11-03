using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PaymentQueries
{
    public class GetAllPaymentsQuery : IRequest<Result<IEnumerable<PaymentDto>>>
    {
    }
}
