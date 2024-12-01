using Application.DTOs;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries.OrderQueries
{
    public class GetFilteredOrdersQuery : IRequest<Result<PagedResult<OrderDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public Status? Status { get; set; }
        public Guid? PaymentId { get; set; }
    }
}
