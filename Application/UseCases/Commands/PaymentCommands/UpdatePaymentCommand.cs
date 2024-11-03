using MediatR;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class UpdatePaymentCommand : IRequest<Unit>
    {
        public Guid PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid UserId { get; set; }
    }
}
