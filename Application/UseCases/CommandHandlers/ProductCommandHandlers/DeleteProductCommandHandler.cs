using Application.UseCases.Commands.ProductCommands;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.ProductCommandHandlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;
        public DeleteProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteProductAsync(request.Id);
            return Unit.Value;
        }
    }
}
