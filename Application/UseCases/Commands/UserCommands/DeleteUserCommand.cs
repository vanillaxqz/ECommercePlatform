using MediatR;

namespace Application.UseCases.Commands.UserCommands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
