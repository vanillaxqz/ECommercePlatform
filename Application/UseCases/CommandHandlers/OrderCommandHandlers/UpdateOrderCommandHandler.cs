using Application.UseCases.Commands.OrderCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.OrderCommandHandlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            await repository.UpdateOrderAsync(order);
            return Unit.Value;
        }
    }
}
