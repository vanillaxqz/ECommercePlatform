using Application.DTOs;
using Application.UseCases.Queries.ProductQueries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Gridify;

namespace Application.UseCases.QueryHandlers.ProductQueryHandlers
{
    public class GetFilteredProductsQueryHandler(IProductRepository repository, IMapper mapper) :
        IRequestHandler<GetFilteredProductsQuery, Result<PagedResult<ProductDto>>>
    {
        public async Task<Result<PagedResult<ProductDto>>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await repository.GetAllProductsAsync();
            var query = products.Data.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(p => p.Name.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(p => p.Description.Contains(request.Description));
            }
            if (request.Price.HasValue)
            {
                query = query.Where(p => p.Price == request.Price.Value);
            }
            if (request.Stock.HasValue)
            {
                query = query.Where(p => p.Stock == request.Stock.Value);
            }
            if (request.Category.HasValue)
            {
                query = query.Where(p => p.Category == request.Category.Value);
            }

            // Apply paging
            var pagedProducts = query.ApplyPaging(request.Page, request.PageSize);
            var productDtos = mapper.Map<List<ProductDto>>(pagedProducts);
            var pagedResult = new PagedResult<ProductDto>(productDtos, query.Count());
            return Result<PagedResult<ProductDto>>.Success(pagedResult);
        }
    }
}
