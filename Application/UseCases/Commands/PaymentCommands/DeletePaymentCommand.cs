using MediatR;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class DeletePaymentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
