using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.ProductCommands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; }
    }
}
