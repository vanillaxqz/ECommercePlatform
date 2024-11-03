using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.OrderQueries
{
    public class GetOrderByUserIdQuery : IRequest<Result<IEnumerable<OrderDto>>>
    {
        public Guid Id { get; set; }
    }
}
