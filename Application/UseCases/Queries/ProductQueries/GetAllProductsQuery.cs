using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
    {
    }
}
