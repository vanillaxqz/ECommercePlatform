using Application.UseCases.Commands.OrderCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.OrderCommandHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            if (order == null)
            {
                return Result<Guid>.Failure("Failure");
            }
            return await repository.AddOrderAsync(order);
        }
    }
}
