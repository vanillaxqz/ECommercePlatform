using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.PaymentCommandHandlers
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Unit>
    {
        private readonly IPaymentRepository repository;
        private readonly IMapper mapper;
        public DeletePaymentCommandHandler(IPaymentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            await repository.DeletePaymentAsync(request.Id);
            return Unit.Value;
        }
    }
}
