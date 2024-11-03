using Application.DTOs;
using Application.UseCases.Queries.OrderQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.OrderQueryHandlers
{
    public class GetOrderByUserIdQueryHandler : IRequestHandler<GetOrderByUserIdQuery, Result<IEnumerable<OrderDto>>>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;
        public GetOrderByUserIdQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await repository.GetOrdersByUserIdAsync(request.Id);
            return mapper.Map<Result<IEnumerable<OrderDto>>>(payments);
        }
    }
}