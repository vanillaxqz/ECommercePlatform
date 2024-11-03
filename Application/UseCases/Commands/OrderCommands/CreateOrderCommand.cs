using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.OrderCommands
{
    public class CreateOrderCommand : IRequest<Result<Guid>>
    {
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public Status Status { get; set; }
        public Guid PaymentId { get; set; }
    }
}
