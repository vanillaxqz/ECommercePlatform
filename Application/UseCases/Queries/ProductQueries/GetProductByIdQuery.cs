using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<Result<ProductDto>>
    {
        public Guid Id { get; set; }
    }
}
