using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public Guid Id { get; set; }
    }
}
