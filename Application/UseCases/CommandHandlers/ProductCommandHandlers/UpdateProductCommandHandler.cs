using Application.UseCases.Commands.ProductCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.ProductCommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;
        public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);
            await repository.UpdateProductAsync(product);
            return Unit.Value;
        }
    }
}
