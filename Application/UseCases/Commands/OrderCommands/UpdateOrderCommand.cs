using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Commands.OrderCommands
{
    public class UpdateOrderCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public Status Status { get; set; }
        public Guid PaymentId { get; set; }
    }
}
