using Application.DTOs;
using Application.UseCases.Queries.OrderQueries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Gridify;

namespace Application.UseCases.QueryHandlers.OrderQueryHandlers
{
    public class GetFilteredOrdersQueryHandler(IOrderRepository repository, IMapper mapper) :
        IRequestHandler<GetFilteredOrdersQuery, Result<PagedResult<OrderDto>>>
    {
        public async Task<Result<PagedResult<OrderDto>>> Handle(GetFilteredOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.GetAllOrdersAsync();
            var query = orders.Data.AsQueryable();

            // Apply filters
            if (request.UserId.HasValue)
            {
                query = query.Where(o => o.UserId == request.UserId.Value);
            }
            if (request.OrderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date == request.OrderDate.Value.Date);
            }
            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status.Value);
            }
            if (request.PaymentId.HasValue)
            {
                query = query.Where(o => o.PaymentId == request.PaymentId.Value);
            }

            // Apply paging
            var pagedOrders = query.ApplyPaging(request.Page, request.PageSize);
            var orderDtos = mapper.Map<List<OrderDto>>(pagedOrders);
            var pagedResult = new PagedResult<OrderDto>(orderDtos, query.Count());
            return Result<PagedResult<OrderDto>>.Success(pagedResult);
        }
    }
}
