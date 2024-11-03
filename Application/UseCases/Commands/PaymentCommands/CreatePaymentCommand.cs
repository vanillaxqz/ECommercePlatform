using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class CreatePaymentCommand : IRequest<Result<Guid>>
    {
        public DateTime PaymentDate { get; set; }
        public Guid UserId { get; set; }
    }
}
