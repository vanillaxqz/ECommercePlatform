using Application.DTOs;
using Application.UseCases.Queries.ProductQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.ProductQueryHandlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;
        public GetAllProductsQueryHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await repository.GetAllProductsAsync();
            return mapper.Map<Result<IEnumerable<ProductDto>>>(products);
        }
    }
}
