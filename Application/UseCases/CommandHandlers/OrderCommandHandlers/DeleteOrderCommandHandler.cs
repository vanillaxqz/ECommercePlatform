using Application.UseCases.Commands.OrderCommands;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.OrderCommandHandlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public DeleteOrderCommandHandler(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteOrderAsync(request.Id);
            return Unit.Value;
        }
    }
}
