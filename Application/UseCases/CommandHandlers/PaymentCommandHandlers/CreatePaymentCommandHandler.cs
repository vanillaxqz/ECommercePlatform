using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.PaymentCommandHandlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
    {
        private readonly IPaymentRepository repository;
        private readonly IMapper mapper;
        public CreatePaymentCommandHandler(IPaymentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = mapper.Map<Payment>(request);
            if (payment == null)
            {
                return Result<Guid>.Failure("Failure");
            }
            return await repository.AddPaymentAsync(payment);
        }
    }
}
