using Application.DTOs;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries.ProductQueries
{
    public class GetFilteredProductsQuery : IRequest<Result<PagedResult<ProductDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public Category? Category { get; set; }
    }
}
