using Application.DTOs;
using Application.UseCases.Queries.ProductQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.ProductQueryHandlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;
        public GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await repository.GetProductByIdAsync(request.Id);
            if (product == null)
            {
                return Result<ProductDto>.Failure("Failure");
            }
            return mapper.Map<Result<ProductDto>>(product);
        }
    }
}
