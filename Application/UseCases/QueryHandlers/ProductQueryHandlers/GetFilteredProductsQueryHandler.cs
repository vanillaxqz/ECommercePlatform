using Application.DTOs;
using Application.UseCases.Queries.ProductQueries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Gridify;
using Domain.Entities;

namespace Application.UseCases.QueryHandlers.ProductQueryHandlers
{
    public class GetFilteredProductsQueryHandler(IProductRepository repository, IMapper mapper) :
        IRequestHandler<GetFilteredProductsQuery, Result<PagedResult<ProductDto>>>
    {
        public async Task<Result<PagedResult<ProductDto>>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            Func<IQueryable<Product>, IQueryable<Product>>? query = products =>
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    products = products.Where(p => p.Name != null && p.Name.Contains(request.Name));
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    products = products.Where(p => p.Description != null && p.Description.Contains(request.Description));
                }
                if (request.Price.HasValue)
                {
                    products = products.Where(p => p.Price == request.Price.Value);
                }
                if (request.Stock.HasValue)
                {
                    products = products.Where(p => p.Stock == request.Stock.Value);
                }
                if (request.Category.HasValue)
                {
                    products = products.Where(p => p.Category == request.Category.Value);
                }
                return products;
            };

            var productsResult = await repository.GetAllProductsAsync(query);
            if (!productsResult.IsSuccess)
            {
                return Result<PagedResult<ProductDto>>.Failure(productsResult.ErrorMessage);
            }

            var products = productsResult.Data.AsQueryable();
            var pagedProducts = products.ApplyPaging(request.Page, request.PageSize);
            var productDtos = mapper.Map<List<ProductDto>>(pagedProducts);
            var pagedResult = new PagedResult<ProductDto>(productDtos, products.Count());
            return Result<PagedResult<ProductDto>>.Success(pagedResult);
        }
    }
}
