using MediatR;

namespace Application.UseCases.Commands.ProductCommands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
