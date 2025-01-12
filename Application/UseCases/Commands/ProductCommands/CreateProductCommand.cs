using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.ProductCommands
{
    public class CreateProductCommand : IRequest<Result<Guid>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; }
        public Guid UserId { get; set; }
    }
}
