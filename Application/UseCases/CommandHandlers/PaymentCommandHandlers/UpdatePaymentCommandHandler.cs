using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.PaymentCommandHandlers
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Unit>
    {
        private readonly IPaymentRepository repository;
        private readonly IMapper mapper;
        public UpdatePaymentCommandHandler(IPaymentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = mapper.Map<Payment>(request);
            await repository.UpdatePaymentAsync(payment);
            return Unit.Value;
        }
    }
}