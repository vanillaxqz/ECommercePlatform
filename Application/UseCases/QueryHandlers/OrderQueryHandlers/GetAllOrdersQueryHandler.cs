using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Order
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<IEnumerable<OrderDto>>>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public GetAllOrdersQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.GetAllOrdersAsync();
            return mapper.Map<Result<IEnumerable<OrderDto>>>(orders);
        }
    }
}
