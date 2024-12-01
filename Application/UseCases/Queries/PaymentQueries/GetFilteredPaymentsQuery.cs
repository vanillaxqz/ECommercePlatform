using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PaymentQueries
{
    public class GetFilteredPaymentsQuery : IRequest<Result<PagedResult<PaymentDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
